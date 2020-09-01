## Release
This is a sound board application built in windows forms. It outputs audio through two selected audio out devices as to allow for the sound to be heard through the application using the sound device and the user themself (through their own local device). This was made to get around all of the unnecessary hoops that downloading a sound board comes with. This project provides a simple and clear way to import, store, and play sounds all at the expense of a 10 mb install. 

### Dependencies
The primary dependency used is [NAudio](https://archive.codeplex.com/?p=naudio). While the documentation of it was lackluster if not for stackoverflow, this provided a specialized and advanced solution to converting audio files and outputing through audio devices. Although the integration was difficult due to the dependency's weak official documentation, I believe the integration of this specifically was crucial to the final product.

![naudio](https://raw.githubusercontent.com/naudio/NAudio/master/naudio-logo.png)

However, with over 100 integrated dependencies the simple to use installer, found in Setup/Release, installs all necessary files imperative to the speed and usage of this product automatically. This build allows for quick and simple navigation to the sound board itself. After installing, 'Soundboard.exe' found in the downloaded project folder is the only file (along with the installed dependencies) needed to run the application.

### Installation

Run Setup.exe in Setup/Release
 - may take a minute to load
 - allow admin
 - select download path
 
Run Soundboard.exe in Download path. This will send a file explorer, select the directory to want to get your sounds from.                                                                          

                                                                              ![soundDirectory](https://lh3.googleusercontent.com/proxy/UELzzlUmSpU5eddQjMKOQYIcW4wle8pBh42RSAntVq0Z10NYNhmdwaAPX1uW0a9rhelPU7WqZEpwsx8uZy7TfwTfGbqE)

**Output:** where you want audio to output (e.g. vb audio cable into microphone)  
**Local Output:** output in which you recieve the audio (e.g. headphones)  

While this is a relativaley easy download, there are still things that a novice user may need to know about this process. Depending on the application you wish to use this on (or if you just want to use it on your desktop) this I would like to provide a guide for the standard user to understand and follow.
  
### Tutorial
If you haven't make sure to read through the installation section of this page to install the app itself. 
  
After this the usage of the app depends on the device _VB-Audio Cable_. This is needed to be able to route one's sounds through their microphone.

To install, go to [VB-Audio Virtual Apps](https://www.vb-audio.com/Cable/) and download the file for your specified OS. 
The download page should look like this: 
![vbInstall](https://gblobscdn.gitbook.com/assets%2F-LNZgv_0q7KdbiUKT8Dm%2F-LNa74oNoEkDdbfZr2U9%2F-LNa832boVO2iYSXjbBf%2Fvb-cable.png?alt=media&token=c67bcbda-88aa-4c4b-abad-a0122a4f8848)
