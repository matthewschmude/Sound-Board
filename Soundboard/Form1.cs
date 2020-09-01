using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using Newtonsoft.Json;
using System.Linq;
using System.Diagnostics;
//using AudioSwitcher.AudioApi.CoreAudio;
//using AudioSwitcher.AudioApi;
//using AudioSwitcher;
//using System.Threading.Tasks;

    //test

namespace Soundboard
{
    public partial class Form1 : Form
    {
        IDictionary<Stopwatch, string> stopwatches = new Dictionary<Stopwatch, string>();
        private SoundDirectory soundDirectory;
        public Form1()
        {
            soundDirectory = new SoundDirectory();
            InitializeComponent();
            initializeJson();
            if (soundDirectory.soundDir == "") getSoundDirectory();
            if (soundDirectory.soundDir != "") getFiles();
            assignDevices();
        }

        private ResFiles resFiles;
        private NAudio.Wave.WaveOut output = null;
        private NAudio.Wave.WaveOut outputLocal = null;
        private NAudio.Wave.BlockAlignReductionStream stream = null;
        private NAudio.Wave.BlockAlignReductionStream stream2 = null;
        private string filePath = null;
        private bool nextBool = false;
        private bool countOver = false;
        private bool pausePlay = true;
        private long totalMil;

        //IEnumerable<CoreAudioDevice> devices = new CoreAudioController().GetPlaybackDevices();
        //
        //public void getOutput()
        //{
        //    IEnumerable<CoreAudioDevice> devices = new CoreAudioController().GetPlaybackDevices();
        //}

        public void soundClick()
        {
            if (filePath != "")
            {
                LoadFile(filePath);
                Stopwatch time = new Stopwatch();
                time.Start();
            }
            else
            {
                MessageBox.Show("No sound files detected.");
            }

            int a = 0;
            while (filePath.IndexOf("\\", a) != -1)
            {
                a = filePath.IndexOf("\\", a) + 1;
            }

            textBox1.Text = filePath.Remove(0, a);
            /*
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Pause();
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Paused) output.Play();
            }
            if (outputLocal != null)
            {
                if (outputLocal.PlaybackState == NAudio.Wave.PlaybackState.Playing) outputLocal.Pause();
                if (outputLocal.PlaybackState == NAudio.Wave.PlaybackState.Paused) outputLocal.Play();
            }
            */
        }
        
        public void LoadFile(string filePath)
        {
            DisposeWave();
            pausePlay = true;

            if (filePath.EndsWith(".mp3"))
            {
                NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(filePath));
                stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
            }
            else if (filePath.EndsWith(".wav"))
            {
                NAudio.Wave.WaveStream pcm = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(filePath));
                stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
            }
            else throw new InvalidOperationException("oh my god just put in the right file type you twat");

            output = new NAudio.Wave.WaveOut();
            output.DeviceNumber = comboBox2.SelectedIndex;

            textBox2.Text = comboBox2.GetItemText(comboBox2.SelectedItem);

            var audioFileReader = new AudioFileReader(filePath);

            string min = Convert.ToInt32(audioFileReader.TotalTime.TotalMinutes).ToString();
            string sec = Convert.ToInt32(audioFileReader.TotalTime.TotalSeconds % 60).ToString();
            string mil = Convert.ToInt32(audioFileReader.TotalTime.TotalMilliseconds % 1000).ToString();
            if (min.Length < 2) min = "0" + min;
            if (sec.Length < 2) sec = "0" + sec;
            if (mil.Length < 2) mil = "00" + mil;
            else if (mil.Length < 3) mil = "0" + mil;

            textBox9.Text = "Total " + min + ":" + sec + ":" + mil;

            audioFileReader.Volume = vol2.Volume;
            output.Init(audioFileReader);
            Stopwatch time = new Stopwatch();
            time.Start();
            stopwatches.Add(time, "time");

            totalMil = Convert.ToInt64(audioFileReader.TotalTime.TotalMilliseconds);

            output.Play();

            if (comboBox1.SelectedIndex != comboBox2.SelectedIndex)
            {
                if (filePath.EndsWith(".mp3"))
                {
                    NAudio.Wave.WaveStream pcm2 = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(filePath));
                    stream2 = new NAudio.Wave.BlockAlignReductionStream(pcm2);
                }
                else if (filePath.EndsWith(".wav"))
                {
                    NAudio.Wave.WaveStream pcm2 = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(filePath));
                    stream2 = new NAudio.Wave.BlockAlignReductionStream(pcm2);
                }
                else throw new InvalidOperationException("Not a compatabible audio file type.");

                outputLocal = new NAudio.Wave.WaveOut();
                outputLocal.DeviceNumber = comboBox1.SelectedIndex;

                textBox4.Text = comboBox1.GetItemText(comboBox1.SelectedItem);

                var audioFileReader2 = new AudioFileReader(filePath);
                audioFileReader2.Volume = volumeSlider1.Volume;
                outputLocal.Init(audioFileReader2);
                outputLocal.Play();

                //float a = volumeSlider1.Volume;
                //outputLocal.Volume = a;
                //outputLocal.Init(stream2);
                //outputLocal.Play();
            } 
        }

        public void assignDevices()
        {
            foreach (KeyValuePair<string, MMDevice> device in GetOutputAudioDevices())
            {
                comboBox1.Items.Add(device.Key);
                comboBox2.Items.Add(device.Key);
            }
        }

        public void getFiles()
        {
            resFiles = new ResFiles();
            int count = 1;
            try
            {
                Directory.EnumerateFiles(soundDirectory.soundDir);
            }
            catch (DirectoryNotFoundException e)
            {
                getSoundDirectory();
            }
            var soundFiles = Directory.EnumerateFiles(soundDirectory.soundDir);
            var fCount = (from file in Directory.EnumerateFiles(@soundDirectory.soundDir, "*", SearchOption.AllDirectories)select file).Count();
            foreach (string currentFile in soundFiles)                                                                
            {
                int a = 0;
                while (currentFile.IndexOf("\\", a) != -1)
                {
                    a = currentFile.IndexOf("\\", a) + 1;
                }
                if (!countOver)
                {
                    if (count == 1 && resFiles.sound1 == "")
                    {
                        resFiles.sound1 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button1.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 2 && resFiles.sound2 == "")
                    {
                        resFiles.sound2 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button2.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 3 && resFiles.sound3 == "")
                    {
                        resFiles.sound3 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button3.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 4 && resFiles.sound4 == "")
                    {
                        resFiles.sound4 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button4.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 5 && resFiles.sound5 == "")
                    {
                        resFiles.sound5 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button5.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 6 && resFiles.sound6 == "")
                    {
                        resFiles.sound6 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button6.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 7 && resFiles.sound7 == "")
                    {
                        resFiles.sound7 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button7.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 8 && resFiles.sound8 == "")
                    {
                        resFiles.sound8 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button8.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 9 && resFiles.sound9 == "")
                    {
                        resFiles.sound9 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button9.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 10 && resFiles.sound10 == "")
                    {
                        resFiles.sound10 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button10.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 11 && resFiles.sound11 == "")
                    {
                        resFiles.sound11 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button11.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 12 && resFiles.sound12 == "")
                    {
                        resFiles.sound12 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button12.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 13 && resFiles.sound13 == "")
                    {
                        resFiles.sound13 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button13.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 14 && resFiles.sound14 == "")
                    {
                        resFiles.sound14 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button14.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 15 && resFiles.sound15 == "")
                    {
                        resFiles.sound15 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button15.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 16 && resFiles.sound16 == "")
                    {
                        resFiles.sound16 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button16.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 17 && resFiles.sound17 == "")
                    {
                        resFiles.sound17 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button17.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 18 && resFiles.sound18 == "")
                    {
                        resFiles.sound18 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button18.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 19 && resFiles.sound19 == "")
                    {
                        resFiles.sound19 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button19.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 20 && resFiles.sound20 == "")
                    {
                        resFiles.sound20 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button20.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 21 && resFiles.sound21 == "")
                    {
                        resFiles.sound21 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button21.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 22 && resFiles.sound22 == "")
                    {
                        resFiles.sound22 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button22.Text = currentFile.Remove(0, a);
                    }
                    else if (count > 22)
                    {
                        if (nextBool == true)
                        {
                            countOver = true;
                            button25.Text = "Last Page";
                        }
                        else break;
                    }
                }
                    
                if (countOver && fCount > 22)
                {
                    if (count == 23 && resFiles.sound23 == "")
                    {
                        resFiles.sound23 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button1.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 24 && resFiles.sound24 == "")
                    {
                        resFiles.sound24 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button2.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 25 && resFiles.sound25 == "")
                    {
                        resFiles.sound25 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button3.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 26 && resFiles.sound26 == "")
                    {
                        resFiles.sound26 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button4.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 27 && resFiles.sound27 == "")
                    {
                        resFiles.sound27 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button5.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 28 && resFiles.sound28 == "")
                    {
                        resFiles.sound28 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button6.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 29 && resFiles.sound29 == "")
                    {
                        resFiles.sound29 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button7.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 30 && resFiles.sound30 == "")
                    {
                        resFiles.sound30 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button8.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 31 && resFiles.sound31 == "")
                    {
                        resFiles.sound31 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button9.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 32 && resFiles.sound32 == "")
                    {
                        resFiles.sound32 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button10.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 33 && resFiles.sound33 == "")
                    {
                        resFiles.sound33 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button11.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 34 && resFiles.sound34 == "")
                    {
                        resFiles.sound34 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button12.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 35 && resFiles.sound35 == "")
                    {
                        resFiles.sound35 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button13.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 36 && resFiles.sound36 == "")
                    {
                        resFiles.sound36 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button14.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 37 && resFiles.sound37 == "")
                    {
                        resFiles.sound37 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button15.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 38 && resFiles.sound38 == "")
                    {
                        resFiles.sound38 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button16.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 39 && resFiles.sound39 == "")
                    {
                        resFiles.sound39 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button17.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 40 && resFiles.sound40 == "")
                    {
                        resFiles.sound40 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button18.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 41 && resFiles.sound41 == "")
                    {
                        resFiles.sound41 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button19.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 42 && resFiles.sound42 == "")
                    {
                        resFiles.sound42 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button20.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 43 && resFiles.sound43 == "")
                    {
                        resFiles.sound43 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button21.Text = currentFile.Remove(0, a);
                    }
                    else if (count == 44 && resFiles.sound44 == "")
                    {
                        resFiles.sound44 = currentFile;
                        textBox3.AppendText("Button " + count.ToString() + ": " + currentFile.Remove(0, a) + System.Environment.NewLine);
                        button22.Text = currentFile.Remove(0, a);
                    }
                    else if (count > 44)
                    {
                        countOver = false;
                        break;
                    }
                }
                else if (nextBool && fCount < 23)
                {
                    button1.Text = "";
                    button2.Text = "";
                    button3.Text = "";
                    button4.Text = "";
                    button5.Text = "";
                    button6.Text = "";
                    button7.Text = "";
                    button8.Text = "";
                    button9.Text = "";
                    button10.Text = "";
                    button11.Text = "";
                    button12.Text = "";
                    button13.Text = "";
                    button14.Text = "";
                    button15.Text = "";
                    button16.Text = "";
                    button17.Text = "";
                    button18.Text = "";
                    button19.Text = "";
                    button20.Text = "";
                    button21.Text = "";
                    button22.Text = "";
                    textBox3.Text = "";
                    break;
                }
                else if (!(fCount > count)) break;
                count++;
            }
        }

        public void DisposeWave()
        {
            try
            {
                if (output != null)
                {
                    if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                    {
                        output.Stop();
                        output.Dispose();
                        output = null;
                    }
                }

                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }

                if (outputLocal != null)
                {
                    if (outputLocal.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                    {
                        outputLocal.Stop();
                        outputLocal.Dispose();
                        outputLocal = null;
                    }
                }

                if (stream2 != null)
                {
                    stream2.Dispose();
                    stream2 = null;
                }
            }

            catch (NAudio.MmException)
            {
                throw;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveSettings();
            DisposeWave();
        }

        public class ResFiles
        {
            public string sound1 { get; set; }
            public string sound2 { get; set; }
            public string sound3 { get; set; }
            public string sound4 { get; set; }
            public string sound5 { get; set; }
            public string sound6 { get; set; }
            public string sound7 { get; set; }
            public string sound8 { get; set; }
            public string sound9 { get; set; }
            public string sound10 { get; set; }
            public string sound11 { get; set; }
            public string sound12 { get; set; }
            public string sound13 { get; set; }
            public string sound14 { get; set; }
            public string sound15 { get; set; }
            public string sound16 { get; set; }
            public string sound17 { get; set; }
            public string sound18 { get; set; }
            public string sound19 { get; set; }
            public string sound20 { get; set; }
            public string sound21 { get; set; }
            public string sound22 { get; set; }
            public string sound23 { get; set; }
            public string sound24 { get; set; }
            public string sound25 { get; set; }
            public string sound26 { get; set; }
            public string sound27 { get; set; }
            public string sound28 { get; set; }
            public string sound29 { get; set; }
            public string sound30 { get; set; }
            public string sound31 { get; set; }
            public string sound32 { get; set; }
            public string sound33 { get; set; }
            public string sound34 { get; set; }
            public string sound35 { get; set; }
            public string sound36 { get; set; }
            public string sound37 { get; set; }
            public string sound38 { get; set; }
            public string sound39 { get; set; }
            public string sound40 { get; set; }
            public string sound41 { get; set; }
            public string sound42 { get; set; }
            public string sound43 { get; set; }
            public string sound44 { get; set; }

            public ResFiles()
            {
                sound1 = "";
                sound2 = "";
                sound3 = "";
                sound4 = "";
                sound5 = "";
                sound6 = "";
                sound7 = "";
                sound8 = "";
                sound9 = "";
                sound10 = "";
                sound11 = "";
                sound12 = "";
                sound13 = "";
                sound14 = "";
                sound15 = "";
                sound16 = "";
                sound17 = "";
                sound18 = "";
                sound19 = "";
                sound20 = "";
                sound21 = "";
                sound22 = "";
                sound23 = "";
                sound24 = "";
                sound25 = "";
                sound26 = "";
                sound27 = "";
                sound28 = "";
                sound29 = "";
                sound30 = "";
                sound31 = "";
                sound32 = "";
                sound33 = "";
                sound34 = "";
                sound35 = "";
                sound36 = "";
                sound37 = "";
                sound38 = "";
                sound39 = "";
                sound40 = "";
                sound41 = "";
                sound42 = "";
                sound43 = "";
                sound44 = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 10; i++)
            {
                Button button = (Button)sender;

                if (!nextBool)
                {
                    if (button.Equals(button1)) filePath = resFiles.sound1;
                    else if (button.Equals(button2)) filePath = resFiles.sound2;
                    else if (button.Equals(button3)) filePath = resFiles.sound3;
                    else if (button.Equals(button4)) filePath = resFiles.sound4;
                    else if (button.Equals(button5)) filePath = resFiles.sound5;
                    else if (button.Equals(button6)) filePath = resFiles.sound6;
                    else if (button.Equals(button7)) filePath = resFiles.sound7;
                    else if (button.Equals(button8)) filePath = resFiles.sound8;
                    else if (button.Equals(button9)) filePath = resFiles.sound9;
                    else if (button.Equals(button10)) filePath = resFiles.sound10;
                    else if (button.Equals(button11)) filePath = resFiles.sound11;
                    else if (button.Equals(button12)) filePath = resFiles.sound12;
                    else if (button.Equals(button13)) filePath = resFiles.sound13;
                    else if (button.Equals(button14)) filePath = resFiles.sound14;
                    else if (button.Equals(button15)) filePath = resFiles.sound15;
                    else if (button.Equals(button16)) filePath = resFiles.sound16;
                    else if (button.Equals(button17)) filePath = resFiles.sound17;
                    else if (button.Equals(button18)) filePath = resFiles.sound18;
                    else if (button.Equals(button19)) filePath = resFiles.sound19;
                    else if (button.Equals(button20)) filePath = resFiles.sound20;
                    else if (button.Equals(button21)) filePath = resFiles.sound21;
                    else if (button.Equals(button22)) filePath = resFiles.sound22;
                }
                else if (nextBool)
                {
                    if (button.Equals(button1)) filePath = resFiles.sound23;
                    else if (button.Equals(button2)) filePath = resFiles.sound24;
                    else if (button.Equals(button3)) filePath = resFiles.sound25;
                    else if (button.Equals(button4)) filePath = resFiles.sound26;
                    else if (button.Equals(button5)) filePath = resFiles.sound27;
                    else if (button.Equals(button6)) filePath = resFiles.sound28;
                    else if (button.Equals(button7)) filePath = resFiles.sound29;
                    else if (button.Equals(button8)) filePath = resFiles.sound30;
                    else if (button.Equals(button9)) filePath = resFiles.sound31;
                    else if (button.Equals(button10)) filePath = resFiles.sound32;
                    else if (button.Equals(button11)) filePath = resFiles.sound33;
                    else if (button.Equals(button12)) filePath = resFiles.sound34;
                    else if (button.Equals(button13)) filePath = resFiles.sound35;
                    else if (button.Equals(button14)) filePath = resFiles.sound36;
                    else if (button.Equals(button15)) filePath = resFiles.sound37;
                    else if (button.Equals(button16)) filePath = resFiles.sound38;
                    else if (button.Equals(button17)) filePath = resFiles.sound39;
                    else if (button.Equals(button18)) filePath = resFiles.sound40;
                    else if (button.Equals(button19)) filePath = resFiles.sound41;
                    else if (button.Equals(button20)) filePath = resFiles.sound42;
                    else if (button.Equals(button21)) filePath = resFiles.sound43;
                    else if (button.Equals(button22)) filePath = resFiles.sound44;
                }
            }
            soundClick();
        }

        public Dictionary<string, MMDevice> GetOutputAudioDevices()
        {
            Dictionary<string, MMDevice> retOut = new Dictionary<string, MMDevice>();
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            int waveOutDevices = NAudio.Wave.WaveOut.DeviceCount;
            for (int waveOutDevice = 0; waveOutDevice < waveOutDevices; waveOutDevice++)
            {
                WaveOutCapabilities deviceInfo = NAudio.Wave.WaveOut.GetCapabilities(waveOutDevice);
                foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Render, NAudio.CoreAudioApi.DeviceState.Active))
                {
                    if (device.ID != null && device.FriendlyName.StartsWith(deviceInfo.ProductName))
                    {
                        retOut.Add(device.FriendlyName, device);
                        break;
                    }
                }
            }
            return retOut;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (soundDirectory.soundDir != "")
            {
                openFileDialog1.ShowDialog();

                foreach (string path in openFileDialog1.FileNames)
                {
                    if (path != "" && path != null)
                    {
                        int a = 0;
                        while (path.IndexOf("\\", a) != -1)
                        {
                            a = path.IndexOf("\\", a) + 1;
                        }
                        string fileName = path.Remove(0, a);
                        string sourcePath = path.Remove(a);
                        string targetPath = @soundDirectory.soundDir;

                        string sourceFile = Path.Combine(sourcePath, fileName);
                        string destFile = Path.Combine(targetPath, fileName);
                        File.Copy(sourceFile, destFile, true);
                    }
                }
            }
            else if (soundDirectory.soundDir == "") MessageBox.Show("No Sound Directory Selected.");
        }

        private void button24_Click(object sender, EventArgs e)
        {
            DisposeWave();
            comboBox1.SelectedIndex = -1;
            comboBox1.Items.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox2.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            button1.Text = "";
            button2.Text = "";
            button3.Text = "";
            button4.Text = "";
            button5.Text = "";
            button6.Text = "";
            button7.Text = "";
            button8.Text = "";
            button9.Text = "";
            button10.Text = "";
            button11.Text = "";
            button12.Text = "";
            button13.Text = "";
            button14.Text = "";
            button15.Text = "";
            button16.Text = "";
            button17.Text = "";
            button18.Text = "";
            button19.Text = "";
            button20.Text = "";
            button21.Text = "";
            button22.Text = "";
            textBox3.Text = "";
            if (soundDirectory.soundDir != "") getFiles();
            assignDevices();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (output != null && output.PlaybackState == NAudio.Wave.PlaybackState.Playing || !pausePlay)
            {
                if (!pausePlay) pausePlay = true;

                output.Stop();
                if (outputLocal != null && outputLocal.PlaybackState == NAudio.Wave.PlaybackState.Playing) outputLocal.Stop();
                textBox9.Text = "";

                foreach (KeyValuePair<Stopwatch, string> time in stopwatches)
                {
                    time.Key.Reset();
                }
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (pausePlay && output != null && output.PlaybackState == NAudio.Wave.PlaybackState.Playing)
            {
                output.Pause();
                if (outputLocal != null && outputLocal.PlaybackState == NAudio.Wave.PlaybackState.Playing) outputLocal.Pause();
                foreach (KeyValuePair<Stopwatch, string> time in stopwatches)
                {
                    long timeE = time.Key.ElapsedMilliseconds;
                    time.Key.Stop();

                    long pLMil = timeE;
                    int pMil = Convert.ToInt32(pLMil);
                    string min = ((pMil / 1000) / 60).ToString();
                    string sec = ((pMil / 1000) % 60).ToString();
                    string mil = (pMil % 1000).ToString();

                    if (min.Length < 2) min = "0" + min;
                    if (sec.Length < 2) sec = "0" + sec;
                    if (mil.Length < 2) mil = "00" + mil;
                    else if (mil.Length < 3) mil = "0" + mil;

                    textBox9.Text = "Paused At " + min + ":" + sec + ":" + mil;
                    /*
                     + " out of " + 
                        ((totalMil / 1000) / 60).ToString().Substring(0, ((totalMil / 1000) / 60).ToString().IndexOf('.'))
                        + ":" + ((totalMil / 1000) % 60).ToString() + ":" + (totalMil % 1000).ToString();
                    */
                } // seconds: totalMil / 1000, mil: totalMil % 1000, (totalMil / 1000) / 60
                pausePlay = false;
            }
            else if (!pausePlay && output != null && output.PlaybackState == PlaybackState.Paused)
            {
                output.Play();
                if (outputLocal != null && outputLocal.PlaybackState == NAudio.Wave.PlaybackState.Paused) outputLocal.Play();
                foreach (KeyValuePair<Stopwatch, string> time in stopwatches)
                {
                    time.Key.Start();
                }
                string upTime = textBox9.Text.Substring(10);
                textBox9.Text = "Unpaused At " + upTime;

                pausePlay = true;
            }
        }
        /*
        private void button27_Click(object sender, EventArgs e)
        {
            if (output != null && output.PlaybackState == NAudio.Wave.PlaybackState.Paused) 
            {
                output.Play();
                foreach (KeyValuePair<Stopwatch, string> time in stopwatches)
                {
                    time.Key.Start();
                }
                string upTime = textBox9.Text.Substring(10);
                textBox9.Text = "Unpaused At " + upTime;
            }
            if (outputLocal != null && outputLocal.PlaybackState == NAudio.Wave.PlaybackState.Paused) outputLocal.Play();
        }
        */
        private void button25_Click(object sender, EventArgs e)
        {
            if (button25.Text == "Next Page")
            {
                button25.Text = "Last Page";
                nextBool = true;
                countOver = false;
                textBox3.Clear();
                if (soundDirectory.soundDir != "") getFiles();
            }
            else if (button25.Text == "Last Page")
            {
                button25.Text = "Next Page";
                countOver = false;
                nextBool = false;
                textBox3.Clear();
                if (soundDirectory.soundDir != "") getFiles();
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
             getSoundDirectory();
        }
        
        public void getSoundDirectory()
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                soundDirectory.soundDir = folderBrowserDialog1.SelectedPath.ToString();
            }
            if (folderBrowserDialog1.SelectedPath == null)
            {
                ;
            }
        }
        
        private void initializeJson()
        {
            try
            {
                soundDirectory = JsonConvert.DeserializeObject<SoundDirectory>(File.ReadAllText(Convert.ToBase64String(Properties.Resources.soundDirect))); // Convert.ToBase64String(Properties.Resources.soundDirect))
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("No memory file detected, generating one..");
                soundDirectory = new SoundDirectory();
                saveSettings();
            }
            catch (JsonReaderException e)
            {
                MessageBox.Show("Corrupt Memory File");
                File.Delete(File.ReadAllText(Convert.ToBase64String(Properties.Resources.soundDirect)));
                this.Close();
            }
        }
        private void saveSettings()
        {
            File.WriteAllText(Convert.ToBase64String(Properties.Resources.soundDirect), JsonConvert.SerializeObject(soundDirectory));
        }
    }
    public class SoundDirectory
    {
        public string soundDir { get; set; }

        public SoundDirectory ()
        {
            soundDir = "";
        }
    }
}
