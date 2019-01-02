using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using ArcSoftFace.SDKModels;
using ArcSoftFace.SDKUtil;
using ArcSoftFace.Utils;
using ArcSoftFace.Entity;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace 人脸识别客户端前端界面
{
    public partial class Form2 : Form
    {
        //引擎Handle
        private IntPtr pEngine = IntPtr.Zero;
        private FilterInfoCollection videoDevices;
        //sdk激活及引擎初始化
        private void InitEngines()
        {
            //读取配置文件
            AppSettingsReader reader = new AppSettingsReader();
            string appId = (string)reader.GetValue("APP_ID", typeof(string));
            string sdkKey64 = (string)reader.GetValue("SDKKEY64", typeof(string));
            string sdkKey32 = (string)reader.GetValue("SDKKEY32", typeof(string));

            var is64CPU = Environment.Is64BitProcess;
            if (is64CPU)
            {
                if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(sdkKey64))
                {
                    /*chooseMultiImgBtn.Enabled = false;
                    matchBtn.Enabled = false;
                    btnClearFaceList.Enabled = false;
                    chooseImgBtn.Enabled = false;*/
                    MessageBox.Show("请在App.config配置文件中先配置APP_ID和SDKKEY64!");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(sdkKey32))
                {
                    /*chooseMultiImgBtn.Enabled = false;
                    matchBtn.Enabled = false;
                    btnClearFaceList.Enabled = false;
                    chooseImgBtn.Enabled = false;*/
                    MessageBox.Show("请在App.config配置文件中先配置APP_ID和SDKKEY32!");
                    return;
                }
            }

            //激活引擎    如出现错误，1.请先确认从官网下载的sdk库已放到对应的bin中，2.当前选择的CPU为x86或者x64
            int retCode = 0;

            try
            {
                retCode = ASFFunctions.ASFActivation(appId, is64CPU ? sdkKey64 : sdkKey32);
            }
            catch (Exception ex)
            {
                /*chooseMultiImgBtn.Enabled = false;
                matchBtn.Enabled = false;
                btnClearFaceList.Enabled = false;
                chooseImgBtn.Enabled = false;*/
                if (ex.Message.IndexOf("无法加载 DLL") > -1)
                {
                    MessageBox.Show("请将sdk相关DLL放入bin对应的x86或x64下的文件夹中!");
                    richTextBox1.AppendText("请将sdk相关DLL放入bin对应的x86或x64下的文件夹中!");

                }
                else
                {
                    MessageBox.Show("激活引擎失败!");
                }
                return;
            }

            Console.WriteLine("Activate Result:" + retCode);

            //初始化引擎
            uint detectMode = DetectionMode.ASF_DETECT_MODE_IMAGE;
            //检测脸部的角度优先值
            int detectFaceOrientPriority = ASF_OrientPriority.ASF_OP_0_HIGHER_EXT;
            //人脸在图片中所占比例，如果需要调整检测人脸尺寸请修改此值，有效数值为2-32
            int detectFaceScaleVal = 16;
            //最大需要检测的人脸个数
            int detectFaceMaxNum = 5;
            //引擎初始化时需要初始化的检测功能组合
            int combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_AGE | FaceEngineMask.ASF_GENDER | FaceEngineMask.ASF_FACE3DANGLE;
            //初始化引擎，正常值为0，其他返回值请参考http://ai.arcsoft.com.cn/bbs/forum.php?mod=viewthread&tid=19&_dsign=dbad527e
            retCode = ASFFunctions.ASFInitEngine(detectMode, detectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMask, ref pEngine);
            Console.WriteLine("InitEngine Result:" + retCode);
            richTextBox1.AppendText((retCode == 0) ? "引擎初始化成功!\n" : string.Format("引擎初始化失败!错误码为:{0}\n", retCode));
            if (retCode != 0)
            {
                /*chooseMultiImgBtn.Enabled = false;
                matchBtn.Enabled = false;
                btnClearFaceList.Enabled = false;
                chooseImgBtn.Enabled = false;*/
            }
        }
        public Form2()
        {
            InitializeComponent();
            InitEngines();
            button5_Click(null, null);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        //打开摄像头
        private void button4_Click(object sender, EventArgs e)
        {
            if (tscbxCameras.SelectedItem.ToString() == "未发现摄像头")
            {
                MessageBox.Show("未发现摄像头", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[tscbxCameras.SelectedIndex].MonikerString);
            videoSource.VideoResolution = videoSource.VideoCapabilities[comboBox1.SelectedIndex];

            videoSourcePlayer.VideoSource = videoSource;
            videoSourcePlayer.Start();
        }
        //关闭摄像头
        private void button5_Click(object sender, EventArgs e)
        {
            if (videoSourcePlayer != null && videoSourcePlayer.IsRunning)
            {
                videoSourcePlayer.SignalToStop();
                videoSourcePlayer.WaitForStop();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                // 枚举所有视频输入设备
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                foreach (FilterInfo device in videoDevices)
                {
                    tscbxCameras.Items.Add(device.Name);
                }

                tscbxCameras.SelectedIndex = 0;

            }
            catch (ApplicationException)
            {
                tscbxCameras.Items.Add("未发现摄像头");
                tscbxCameras.SelectedIndex = 0;
                videoDevices = null;
            }
        }
        //拍照
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (videoSourcePlayer.IsRunning)
                {
                    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                    videoSourcePlayer.GetCurrentVideoFrame().GetHbitmap(),
                                    IntPtr.Zero,
                                     Int32Rect.Empty,
                                    BitmapSizeOptions.FromEmptyOptions());
                    PngBitmapEncoder pE = new PngBitmapEncoder();
                    pE.Frames.Add(BitmapFrame.Create(bitmapSource));
                    string picName = GetImagePath() + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                    if (File.Exists(picName))
                    {
                        File.Delete(picName);
                    }
                    using (Stream stream = File.Create(picName))
                    {
                        pE.Save(stream);
                    }
                    //显示在picturebox里
                    pic.Load(picName);
                    /*特征值提取*/
                    ASF_SingleFaceInfo singleFaceInfo = new ASF_SingleFaceInfo();
                    IntPtr feature = FaceUtil.ExtractFeature(pEngine, Image.FromFile(picName), out singleFaceInfo);
                    this.Invoke(new Action(delegate
                    {
                        if (singleFaceInfo.faceRect.left == 0 && singleFaceInfo.faceRect.right == 0)
                        {
                            richTextBox1.AppendText(string.Format("未检测到人脸\r\n"));
                        }
                        else
                        {
                            richTextBox1.AppendText(string.Format("已提取人脸特征值，[left:{1},right:{2},top:{3},bottom:{4},orient:{5}]\r\n", 1, singleFaceInfo.faceRect.left, singleFaceInfo.faceRect.right, singleFaceInfo.faceRect.top, singleFaceInfo.faceRect.bottom, singleFaceInfo.faceOrient));

                            ASF_FaceFeature facefeature = MemoryUtil.PtrToStructure<ASF_FaceFeature>(feature);
                            byte[] feature1 = new byte[facefeature.featureSize];
                            MemoryUtil.Copy(facefeature.feature, feature1, 0, facefeature.featureSize);
                            Console.WriteLine(feature1.LongLength);//显示feature1.长度
                        }
                    }));
                }
                else
                    MessageBox.Show("当前尚未连接摄像头", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (Exception ex)
            {
                MessageBox.Show("摄像头异常：" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
    }

        //获取路径
        private string GetImagePath()
        {
            string personImgPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "Img";
            if (!Directory.Exists(personImgPath))
            {
                Directory.CreateDirectory(personImgPath);
            }

            return personImgPath;
        }
        //调整分辨率
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                    button5_Click(null, null);
                if (tscbxCameras.SelectedItem.ToString() != "未发现摄像头")
                    button4_Click(null,null);
            
        }
        //调整摄像头
        private void tscbxCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            button5_Click(null, null);
            if (tscbxCameras.SelectedItem.ToString() == "未发现摄像头")
            {
                comboBox1.Items.Add("未发现摄像头");
                comboBox1.SelectedIndex = 0;
                return;
            }
            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[tscbxCameras.SelectedIndex].MonikerString);
            if (videoSource.VideoCapabilities.Count() == 0)
            {
                comboBox1.Items.Add("摄像头异常");
                comboBox1.SelectedIndex = 0;
                return;
            }
            comboBox1.Items.Clear();
            foreach (AForge.Video.DirectShow.VideoCapabilities FBL in videoSource.VideoCapabilities)
            {
                comboBox1.Items.Add(FBL.FrameSize.Width + "*" + FBL.FrameSize.Height);
            }

            comboBox1.SelectedIndex = 0;
            button4_Click(null, null);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            button5_Click(null, null);
            System.Environment.Exit(0);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
