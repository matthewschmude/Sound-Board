## Release
This is a sound board application built in windows forms. It outputs audio through two selected audio out devices as to allow for the sound to be heard through the application using the sound device and the user themself (through their own local device). This was made to get around all of the unnecessary hoops that downloading a sound board comes with. This project provides a simple and clear way to import, store, and play sounds all at the expense of a 10 mb install. 

### Dependencies
The primary dependency used is [NAudio](https://archive.codeplex.com/?p=naudio). While the documentation of it was lackluster if not for stackoverflow, this provided a specialized and advanced solution to converting audio files and outputing through audio devices. Although the integration was difficult due to the dependency's weak official documentation, I believe the integration of this specifically was crucial to the final product.

However, with over 100 integrated dependencies the simple to use installer, found in Setup/Release, installs all necessary files imperative to the speed and usage of this product automatically. This build allows for quick and simple navigation to the sound board itself. After installing, 'Soundboard.exe' found in the downloaded project folder is the only file (along with the installed dependencies) needed to run the application.

### Installation

Run Setup.exe in Setup/Release
 - may take a minute to load
 - allow admin
 - select download path
 
Run Soundboard.exe in Download path. This will send a file explorer, select the directory to want to get your sounds from.

**Output:** where you want audio to output (e.g. vb audio cable into microphone)  
**Local Output:** output in which you recieve the audio (e.g. headphones)  

While this is a relativaley easy download, there are still things that a novice user may need to know about this process. Depending on the application you wish to use this on (or if you just want to use it on your desktop) this I would like to provide a guide for the standard user to understand and follow.
  
### Tutorial
To start, go through all the steps in the [installation]() section of this page.
