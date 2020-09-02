## About
This is a sound board application built in windows forms. It outputs audio through two selected audio out devices as to allow for the sound to be heard through the application using the sound device and the user themself (through their own local device). This was made to get around all of the unnecessary hoops that downloading a sound board comes with. This project provides a simple and clear way to import, store, and play sounds all at the expense of a 10 mb install. 

If any bugs or random crashes are encountered, feel free to comment or send a pull-request on the [github provided].

### Dependencies
The primary dependency used is [NAudio](https://archive.codeplex.com/?p=naudio). While the documentation of it was lackluster if not for stackoverflow, this provided a specialized and advanced solution to converting audio files and outputing through audio devices. Although the integration was difficult due to the dependency's weak official documentation, I believe the integration of this specifically was crucial to the final product.

<p align="center">
  <img width="619" height="143" src="https://raw.githubusercontent.com/naudio/NAudio/master/naudio-logo.png">
</p>

However, with over 100 integrated dependencies the simple to use installer, found in Setup/Release, installs all necessary files imperative to the speed and usage of this product automatically. This build allows for quick and simple navigation to the sound board itself. After installing, 'Soundboard.exe' found in the downloaded project folder is the only file (along with the installed dependencies) needed to run the application.

### Installation

Run Setup.exe in Setup/Release
 - may take a minute to load
 - allow admin
 - select download path
 
Run Soundboard.exe in Download path. This will send a file explorer, select the directory to want to get your sounds from.
  
<p align="center">
  <img width="326" height="363" src="https://lh3.googleusercontent.com/proxy/UELzzlUmSpU5eddQjMKOQYIcW4wle8pBh42RSAntVq0Z10NYNhmdwaAPX1uW0a9rhelPU7WqZEpwsx8uZy7TfwTfGbqE">
</p>
  
**Output:** where you want audio to output (e.g. vb audio cable into microphone)  
**Local Output:** output in which you recieve the audio (e.g. headphones)  

While this is a relativaley easy download, there are still things that a novice user may need to know about this process. Depending on the application you wish to use this on (or if you just want to use it on your desktop) this I would like to provide a guide for the standard user to understand and follow.
  
### Tutorial
If you haven't make sure to read through the installation section of this page to install the app itself. 
  
After this the usage of the app depends on the device _VB-Audio Cable_. This is needed to be able to route one's sounds through their microphone.

To install, go to [VB-Audio Virtual Apps](https://www.vb-audio.com/Cable/) and download the file for your specified OS. 
The download page should look like this: 

![vbInstall](https://gblobscdn.gitbook.com/assets%2F-LNZgv_0q7KdbiUKT8Dm%2F-LNa74oNoEkDdbfZr2U9%2F-LNa832boVO2iYSXjbBf%2Fvb-cable.png?alt=media&token=c67bcbda-88aa-4c4b-abad-a0122a4f8848)
  
Follow the steps in the installation wizard to download the two audio devices included in _VB-Audio Cable_.

To set up the audio cable:
  - Open Sound Settings and navigate to 'Sound Control Panel'.  
  - Now select the 'Recording' tab located to the right of the 'Playback' Tab
  - Find your main Audio Output device you are using for your application and navigate to its properties
  - Next select the 'Listen' Tab at the top, just to the right of the 'General' tab
  - Check the box which says "Listen to this device" and in the drop down menu below, select the VB-Audio Cable Input
  - Apply settings and exit
  
<p align="center">
  <img width="512" height="382" src="https://lh3.googleusercontent.com/proxy/dxTu3OcLQMV4Bhfhr7vgDmrSMnH_DlOThJ2wvxH27MpzwWZb_d3nNVvtANqKxq9gbaajBV_79dacrxmZesXj47aWiZG3d6ZkYK9n1JdfgYJvxXpn0gnWviwhtM0O_OXnRCLGqv6jM78a2sAxt3fc8q_lOBE1xzR5dU9_c6wNGsJ5xgc7mWHlsolz4o-585vkfKuZtvqj2A">
</p>
 
The last step is to just change the audio input device whatever application you wish to use the sound board on to the VB-Audio Output Device.

Personally, I would reccomend messing around with the input volume of the VB-Audio Cable as depending on the device, the volume of your microphone inputs may be increased or decreased while being sent through the devices.
  
This process works by listening to the audio from you microphone on the VB-Audio Input Device, then sending all the audio from that device to the VB-Audio Output Device.
This intuitive design allows the user to select the VB-Audio Input Device in the sound board application to send sounds through - those of which are sent to VB-Audio Output, along with any incoming sound from one's microphone, and thus put through the input device of whatever application has VB-Audio Output selected.

### Use Cases
This app can be used in tandem with VB-Audio Cable in all applications that require an audio input device, such as Discord, Skype, Google Meets, Zoom, Microsoft Teams, and more.

<p align="center">
  <img src="https://discord.com/assets/2c21aeda16de354ba5334551a883b481.png" width="49" height="50" />, <img src="https://1000logos.net/wp-content/uploads/2017/06/Skype-Logo.png" width="273" height="119" />
</p>

