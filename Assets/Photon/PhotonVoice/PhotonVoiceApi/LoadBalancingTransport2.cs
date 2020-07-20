// -----------------------------------------------------------------------
// <copyright file="LoadBalancingTransport2.cs" company="Exit Games GmbH">
//   Photon Voice API Framework for Photon - Copyright (C) 2020 Exit Games GmbH
// </copyright>
// <summary>
//   Extends Photon Realtime API with audio streaming functionality.
// </summary>
// <author>developer@photonengine.com</author>
// ----------------------------------------------------------------------------


namespace Photon.Voice
{
    using System;
    using ExitGames.Client.Photon;
    using Realtime;


    /// <summary>
    /// Variant of LoadBalancingTransport. Aims to be non-alloc at the cost of breaking compatibility with older clients.
    /// </summary>
    public class LoadBalancingTransport2 : LoadBalancingTransport
    {
        public LoadBalancingTransport2(ConnectionProtocol connectionProtocol = ConnectionProtocol.Udp) : base(connectionProtocol)
        {
            this.LoadBalancingPeer.UseByteArraySlicePoolForEvents = true; // incoming byte[] events can be deserialized to a pooled ByteArraySlice
            this.LoadBalancingPeer.ReuseEventInstance = true;             // this won't store references to the event anyways
        }


        public override void SendFrame(ArraySegment<byte> data, byte evNumber, byte voiceId, int channelId, LocalVoice localVoice)
        {
            // this uses a pooled slice, which is released within the send method (here RaiseEvent at the bottom)
            ByteArraySlice frameData = this.LoadBalancingPeer.ByteArraySlicePool.Acquire(data.Count + 2);
            frameData.Buffer[0] = voiceId;
            frameData.Buffer[1] = evNumber;
            Buffer.BlockCopy(data.Array, 0, frameData.Buffer, 2, data.Count);
            frameData.Count = data.Count + 2; // need to set the count, as we manipulated the buffer directly

            SendOptions sendOpt = new SendOptions() { Reliability = localVoice.Reliable, Channel = this.photonChannelForCodec(localVoice.info.Codec), Encrypt = localVoice.Encrypt };

            RaiseEventOptions opt = new RaiseEventOptions();
            if (localVoice.DebugEchoMode)
            {
                opt.Receivers = ReceiverGroup.All;
            }

            opt.InterestGroup = localVoice.InterestGroup;

            this.OpRaiseEvent(VoiceEvent.FrameCode, frameData, opt, sendOpt);

            // each voice has it's own connection? else, we could aggregate voices data in less count of datagrams
            this.LoadBalancingPeer.SendOutgoingCommands();
        }


        protected override void onEventActionVoiceClient(EventData ev)
        {
            if (ev.Code == VoiceEvent.FrameCode)
            {
                // Payloads are arrays. If first array element is 0 than next is event subcode. Otherwise, the event is data frame with voiceId in 1st element.
                this.onVoiceFrameEvent(ev[(byte)ParameterCode.CustomEventContent], VOICE_CHANNEL, (int)ev[ParameterCode.ActorNr], this.LocalPlayer.ActorNumber);
            }
            else
            {
                base.onEventActionVoiceClient(ev);
            }
        }


        internal void onVoiceFrameEvent(object content0, int channelId, int playerId, int localPlayerId)
        {
            byte[] content = null;
            int contentLength = 0;
            ByteArraySlice slice = content0 as ByteArraySlice;
            if (slice != null)
            {
                content = slice.Buffer;
                contentLength = slice.Count;
            }
            else
            {
                content = content0 as byte[];
                contentLength = content.Length;
            }

            if (content == null || contentLength < 3)
            {
                this.LogError("[PV] onVoiceFrameEvent did not receive data (readable as byte[]) " + content0);
            }
            else
            {
                byte voiceId = (byte)content[0];
                byte evNumber = (byte)content[1];

                byte[] receivedBytes = new byte[contentLength - 2]; // TODO: pool this and release when decoded (problem: size is different for most frames)
                Buffer.BlockCopy(content, 2, receivedBytes, 0, receivedBytes.Length);

                if (slice != null)
                {
                    slice.Release();
                }

                this.voiceClient.onFrame(channelId, playerId, voiceId, evNumber, receivedBytes, playerId == localPlayerId);
            }
        }
    }
}