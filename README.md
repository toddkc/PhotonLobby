# PhotonLobby

This project is being created to help me learn how to make a multiplayer game.  I have a dream of making a game where vr-players and non-vr-players alike can join each other in PVP and PVE experiences.

To install:

Install Unity 2019.4 LTS.  Create a Photon account and get an app id for PUN2 and Voice2.  Enter them into the Photon Server Settings.  I have the minimum required code for Photon and Oculus already in this repo.

For a non-vr build you will want the following scenes in your build index:
MainMenu.scene
Lobby.scene
Game.scene

For a vr build you want the same, but with the VR versions of the scenes.

The project settings included use legacy vr, if you want to use the newer XR you will need to update the settings and packages.

I am using a free skybox from the asset store, but it was large and I didn't want to include it in the repo, so you'll probably want to get one to replace the default Unity skybox.

To Play:

There isn't actually much you can do now.  You join the main menu scene, where you can join a random room.  Once inside a room you can chat with others in the room.  From the room lobby you can load a game scene, which serves as an example showing how you would have all the players in your room play various games together.  Load game scene, play game, load room lobby, repeat...
