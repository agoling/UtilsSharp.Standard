using System;
using System.Drawing;
using System.Security.Cryptography;

namespace UtilsSharp
{
    /// <summary>
    /// 验证图片类
    /// </summary>
    public class VerificationCodeHelper
    {
        #region 私有字段

        private string _text;
        private Bitmap _image;
        private static readonly byte[] RandByte = new byte[4];
        private static readonly RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();

        private readonly Font[] _fonts =
        {
            new Font(new FontFamily("Times New Roman"), 10 + Next(1), FontStyle.Regular),
            new Font(new FontFamily("Georgia"), 10 + Next(1), FontStyle.Regular),
            new Font(new FontFamily("Arial"), 10 + Next(1), FontStyle.Regular),
            new Font(new FontFamily("Comic Sans MS"), 10 + Next(1), FontStyle.Regular)
        };

        #endregion

        #region 公有属性

        /// <summary>
        /// 验证码
        /// </summary>
        public string Text => _text;

        /// <summary>
        /// 验证码图片
        /// </summary>
        public Bitmap Image => _image;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public VerificationCodeHelper(VerificationCodeRule rule)
        {
            CreateImage(rule);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        private static int Next(int max)
        {
            Rand.GetBytes(RandByte);
            int value = BitConverter.ToInt32(RandByte, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 绘制验证码
        /// </summary>
        public void CreateImage(VerificationCodeRule rule)
        {
            _text = RandomHelper.Number(rule.LetterCount);
            int intImageWidth = _text.Length * rule.LetterWidth;
            Bitmap dstImage = new Bitmap(intImageWidth, rule.LetterHeight);
            Graphics g = Graphics.FromImage(dstImage);
            g.Clear(Color.White);
            for (int i = 0; i < 2; i++)
            {
                int x1 = Next(dstImage.Width - 1);
                int x2 = Next(dstImage.Width - 1);
                int y1 = Next(dstImage.Height - 1);
                int y2 = Next(dstImage.Height - 1);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            int px = -12;
            for (int intIndex = 0; intIndex < this._text.Length; intIndex++)
            {
                px += Next(12, 16);
                var py = Next(-2, 2);
                string strChar = this._text.Substring(intIndex, 1);
                strChar = Next(1) == 1 ? strChar.ToLower() : strChar.ToUpper();
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(px, py);
                g.DrawString(strChar, _fonts[Next(_fonts.Length - 1)], newBrush, thePos);
            }

            for (int i = 0; i < 10; i++)
            {
                int x = Next(dstImage.Width - 1);
                int y = Next(dstImage.Height - 1);
                dstImage.SetPixel(x, y, Color.FromArgb(Next(0, 255), Next(0, 255), Next(0, 255)));
            }

            dstImage = TwistImage(dstImage, true, Next(1, 3), Next(4, 6));
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, intImageWidth - 1, rule.LetterHeight - 1);
            _image = dstImage;
        }

        /// <summary>
        /// 字体随机颜色
        /// </summary>
        public Color GetRandomColor()
        {
            Random randomNumFirst = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(randomNumFirst.Next(50));
            Random randomNumSecond = new Random((int)DateTime.Now.Ticks);
            int intRed = randomNumFirst.Next(180);
            int intGreen = randomNumSecond.Next(180);
            int intBlue = (intRed + intGreen > 300) ? 0 : 400 - intRed - intGreen;
            intBlue = (intBlue > 255) ? 255 : intBlue;
            return Color.FromArgb(intRed, intGreen, intBlue);
        }

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="waveformValue">波形的幅度倍数，越大扭曲的程度越高,一般为3</param>
        /// <param name="dPhase">波形的起始相位,取值区间[0-2*PI)</param>
        public Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double waveformValue, double dPhase)
        {
            double PI = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? destBmp.Height : destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    var dx = bXDir ? (PI * j) / dBaseAxisLen : (PI * i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    var nOldX = bXDir ? i + (int)(dy * waveformValue) : i;
                    var nOldY = bXDir ? j : j + (int)(dy * waveformValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                                   && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            srcBmp.Dispose();
            return destBmp;
        }

        #endregion
    }

    /// <summary>
    /// 验证码生成规则
    /// </summary>
    public class VerificationCodeRule
    {
        /// <summary>
        /// 验证码位数
        /// </summary>
        public int LetterCount { set; get; } = 4;

        /// <summary>
        /// 单个字体的宽度范围
        /// </summary>
        public int LetterWidth { set; get; } = 16;

        /// <summary>
        /// 单个字体的高度范围
        /// </summary>
        public int LetterHeight { set; get; } = 20;
    }
}
