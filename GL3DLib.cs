﻿using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Wpf3DLib
{
    /// <summary>
    /// OpenGLを使って3Dデータを標示するライブラリ
    /// 
    /// OpenTK,OpenTK.GLControlをNuGetでインストール
    /// System.Drawingを参照に追加
    /// System.Windows.Formsを参照に追加
    /// </summary>
    public class GL3DLib
    {
        //  文字データ
        private float[][,] mMarkFont = new float[15][,];        //  記号 !"#$%&'()*+,-./
        private float[][,] mNumberFont = new float[10][,];      //  数時 0123456789
        private float[][,] mLargeAlphaFont = new float[26][,];  //  大文字 A-Z
        private float[][,] mSmallAlphaFont = new float[26][,];  //  小文字 a-z
                                                                //  未サポート   :;<=>?@ [\]^_` {|}~
        private float mFontSize = 1.0f;
        private int[,] mFontFaceData = {            //  文字の向き
            { 0, 1, 2 },                            //  XY平面上
            { 2, 0, 1 },                            //  YZ平面上
            { 1, 2, 0 },                            //  ZX平面上
            { 1, 0, 2 },                            //  YX平面上
            { 2, 1, 0 },                            //  ZY平面上
            { 0, 2, 1 }                             //  XZ平面上
        };
        public enum FONTFACE { XY, YZ, ZX, YX, ZY, XZ }
        private FONTFACE mFontFace = FONTFACE.XZ;

        //  色
        public Color4[] mColor4 = {
            Color4.AliceBlue, Color4.AntiqueWhite, Color4.Aqua, Color4.Aquamarine, Color4.Azure,
            Color4.Beige, Color4.Bisque, Color4.Black, Color4.BlanchedAlmond, Color4.Blue,
            Color4.BlueViolet, Color4.Brown, Color4.BurlyWood, Color4.CadetBlue, Color4.Chartreuse,
            Color4.Chocolate, Color4.Coral, Color4.CornflowerBlue, Color4.Cornsilk, Color4.Crimson,
            Color4.Cyan, Color4.DarkBlue, Color4.DarkCyan, Color4.DarkGoldenrod, Color4.DarkGray,
            Color4.DarkGreen, Color4.DarkKhaki, Color4.DarkMagenta, Color4.DarkOliveGreen, Color4.DarkOrange,
            Color4.DarkOrchid,Color4.DarkRed, Color4.DarkSalmon,Color4.DarkSeaGreen, Color4.DarkSlateBlue,
            Color4.DarkSlateGray,Color4.DarkTurquoise, Color4.DarkViolet, Color4.DeepPink, Color4.DeepSkyBlue,
            Color4.DimGray,Color4.DodgerBlue, Color4.Firebrick, Color4.FloralWhite, Color4.ForestGreen,
            Color4.Fuchsia, Color4.Gainsboro, Color4.GhostWhite,Color4.Gold, Color4.Goldenrod,
            Color4.Gray, Color4.Green, Color4.GreenYellow, Color4.Honeydew, Color4.HotPink,
            Color4.IndianRed, Color4.Indigo, Color4.Ivory, Color4.Khaki, Color4.Lavender,
            Color4.LavenderBlush, Color4.LawnGreen, Color4.LemonChiffon, Color4.LightBlue, Color4.LightCoral,
            Color4.LightCyan, Color4.LightGoldenrodYellow, Color4.LightGray, Color4.LightGreen, Color4.LightPink,
            Color4.LightSalmon, Color4.LightSeaGreen, Color4.LightSkyBlue, Color4.LightSlateGray, Color4.LightSteelBlue,
            Color4.LightYellow, Color4.Lime, Color4.LimeGreen, Color4.Linen, Color4.Magenta,
            Color4.Maroon, Color4.MediumAquamarine, Color4.MediumBlue, Color4.MediumOrchid, Color4.MediumPurple,
            Color4.MediumSeaGreen, Color4.MediumSlateBlue, Color4.MediumSpringGreen, Color4.MediumTurquoise, Color4.MediumVioletRed,
            Color4.MidnightBlue, Color4.MintCream, Color4.MistyRose, Color4.Moccasin, Color4.NavajoWhite,
            Color4.Navy, Color4.OldLace, Color4.Olive, Color4.OliveDrab, Color4.Orange,
            Color4.OrangeRed, Color4.Orchid, Color4.PaleGoldenrod, Color4.PaleGreen, Color4.PaleTurquoise,
            Color4.PaleVioletRed, Color4.PapayaWhip, Color4.PeachPuff, Color4.Peru, Color4.Pink,
            Color4.Plum, Color4.PowderBlue, Color4.Purple, Color4.Red, Color4.RosyBrown,
            Color4.RoyalBlue, Color4.SaddleBrown,Color4.Salmon, Color4.SandyBrown, Color4.SeaGreen,
            Color4.SeaShell, Color4.Sienna, Color4.Silver, Color4.SkyBlue, Color4.SlateBlue,
            Color4.SlateGray, Color4.Snow, Color4.SpringGreen, Color4.SteelBlue, Color4.Tan,
            Color4.Teal, Color4.Thistle, Color4.Tomato, Color4.Transparent, Color4.Turquoise,
            Color4.Violet, Color4.Wheat, Color4.White, Color4.WhiteSmoke, Color4.Yellow,
            Color4.YellowGreen
        };
        public string[] mColor4Title = {
            "AliceBlue", "AntiqueWhite", "Aqua", "Aquamarine", "Azure",
            "Beige", "Bisque", "Black", "BlanchedAlmond", "Blue",
            "BlueViolet", "Brown", "BurlyWood", "CadetBlue", "Chartreuse",
            "Chocolate", "Coral", "CornflowerBlue", "Cornsilk", "Crimson",
            "Cyan", "DarkBlue", "DarkCyan", "DarkGoldenrod", "DarkGray",
            "DarkGreen", "DarkKhaki", "DarkMagenta", "DarkOliveGreen", "DarkOrange",
            "DarkOrchid","DarkRed", "DarkSalmon","DarkSeaGreen", "DarkSlateBlue",
            "DarkSlateGray","DarkTurquoise", "DarkViolet", "DeepPink", "DeepSkyBlue",
            "DimGray","DodgerBlue", "Firebrick", "FloralWhite", "ForestGreen",
            "Fuchsia", "Gainsboro", "GhostWhite","Gold", "Goldenrod",
            "Gray", "Green", "GreenYellow", "Honeydew", "HotPink",
            "IndianRed", "Indigo", "Ivory", "Khaki", "Lavender",
            "LavenderBlush", "LawnGreen", "LemonChiffon", "LightBlue", "LightCoral",
            "LightCyan", "LightGoldenrodYellow", "LightGray", "LightGreen", "LightPink",
            "LightSalmon", "LightSeaGreen", "LightSkyBlue", "LightSlateGray", "LightSteelBlue",
            "LightYellow", "Lime", "LimeGreen", "Linen", "Magenta",
            "Maroon", "MediumAquamarine", "MediumBlue", "MediumOrchid", "MediumPurple",
            "MediumSeaGreen", "MediumSlateBlue", "MediumSpringGreen", "MediumTurquoise", "MediumVioletRed",
            "MidnightBlue", "MintCream", "MistyRose", "Moccasin", "NavajoWhite",
            "Navy", "OldLace", "Olive", "OliveDrab", "Orange",
            "OrangeRed", "Orchid", "PaleGoldenrod", "PaleGreen", "PaleTurquoise",
            "PaleVioletRed", "PapayaWhip", "PeachPuff", "Peru", "Pink",
            "Plum", "PowderBlue", "Purple", "Red", "RosyBrown",
            "RoyalBlue", "SaddleBrown","Salmon", "SandyBrown", "SeaGreen",
            "SeaShell", "Sienna", "Silver", "SkyBlue", "SlateBlue",
            "SlateGray", "Snow", "SpringGreen", "SteelBlue", "Tan",
            "Teal", "Thistle", "Tomato", "Transparent", "Turquoise",
            "Violet", "Wheat", "White", "WhiteSmoke", "Yellow",
            "YellowGreen"
        };


        private Vector3 mMin;                       //  表示エリアの最小値
        private Vector3 mMax;                       //  表示エリアの最大値
        private Vector3 mDis;                       //  表示エリアの大きさ
        private Color4 mBackColor = Color4.White;   //  背景色
        public bool mIsSurfaceModel = false;        //  SureFaceModelかWireFrameかの選択
        public bool mAxisFrameDisp = true;          //  表示領域の枠表示
        public bool mAxisDisp = true;               //  原点軸表示
        private double mColorMin;
        private double mColorMax;

        private bool mIsCameraRotating;             //  カメラが回転状態かどうか
        private bool mIsTransrate;                  //  移動状態
        private Vector2 mCcurrent, mPrevious;       //  現在の点、前の点
        private Matrix4 mRotate;                    //  回転行列
        private float mZoom;                        //  拡大度
        public float mZoomMax = 2.0f;               //  最大拡大率
        public float mZoomMin = 0.5f;               //  最小拡大率

        private GLControl mGlControl;               //  OpenTK.GLcontrol

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gLControl">OpenTKのGLControl</param>
        public GL3DLib(GLControl gLControl)
        {
            mGlControl = gLControl;
            FontInit();
        }

        /// <summary>
        /// 表示位置関係を初期化する 
        /// </summary>
        /// <param name="zoom">拡大率</param>
        /// <param name="xrotate">X軸回転(deg)</param>
        /// <param name="yrotate">Y軸回転(deg)</param>
        /// <param name="zrotate">Z軸回転(deg)</param>
        public void initPosition(float zoom, float xrotate, float yrotate, float zrotate)
        {
            mIsCameraRotating = false;
            mIsTransrate = false;
            mCcurrent = Vector2.Zero;
            mPrevious = Vector2.Zero;
            mRotate = Matrix4.Identity;
            mZoom = zoom;
            //  XYZ軸を中心回転
            Quaternion after = new Quaternion(xrotate / 180f * (float)Math.PI, yrotate / 180f * (float)Math.PI, zrotate / 180f * (float)Math.PI);
            mRotate *= Matrix4.CreateFromQuaternion(after);
            //  移動の追加
            //mRotate *= Matrix4.CreateTranslation(0f, 0f, 0f);
        }

        /// <summary>
        /// 移動の追加 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void initMove(float x, float y, float z)
        {
            mRotate *= Matrix4.CreateTranslation(x, y, z);
        }

        /// <summary>
        /// 拡大率の設定
        /// </summary>
        /// <param name="zoom"></param>
        public void setZoom(float zoom)
        {
            mZoom *= (float)Math.Pow(1.2, zoom);
            if (mZoomMax < mZoom)
                mZoom = mZoomMax;
            if (mZoom < mZoomMin)
                mZoom = mZoomMin;
        }

        /// <summary>
        /// 視点(カメラ)の移動開始
        /// 通常はglControl_MouseDown()から呼ばれ、マウスの開始位置を指定する
        /// </summary>
        /// <param name="isRotate">回転/移動の選択</param>
        /// <param name="x">開始X座標</param>
        /// <param name="y">開始Y座標</param>
        public void setMoveStart(bool isRotate, float x, float y)
        {
            if (isRotate)
                mIsCameraRotating = true;
            else
                mIsTransrate = true;
            mCcurrent = new Vector2(x, y);
        }

        /// <summary>
        /// 視点(カメラ)の移動終了
        /// 通常はglControl_MouseUp()から呼ばれる
        /// </summary>
        public void setMoveEnd()
        {
            mIsCameraRotating = false;
            mIsTransrate = false;
            mPrevious = Vector2.Zero;
        }

        /// <summary>
        /// 視点(カメラ)の移動処理
        /// 通常はglControl_MosueMove()から呼ばれる
        /// </summary>
        /// <param name="x">移動X座標</param>
        /// <param name="y">移動Y座標</param>
        /// <returns></returns>
        public bool moveObject(float x, float y)
        {
            if (mIsCameraRotating) {
                //  回転
                mPrevious = mCcurrent;
                mCcurrent = new Vector2(x, y);
                Vector2 delta = mCcurrent - mPrevious;
                delta /= (float)Math.Sqrt(mGlControl.Width * mGlControl.Width + mGlControl.Height * mGlControl.Height);
                float length = delta.Length;
                if (0.0 < length) {
                    float rad = length * MathHelper.Pi;
                    float theta = (float)Math.Sin(rad) / length;
                    Quaternion after = new Quaternion(delta.Y * theta, delta.X * theta, 0.0f, (float)Math.Cos(rad));
                    mRotate *= Matrix4.CreateFromQuaternion(after);
                }
            } else if (mIsTransrate) {
                //  移動
                mPrevious = mCcurrent;
                mCcurrent = new Vector2(x, y);
                Vector2 delta = mCcurrent - mPrevious;
                mRotate *= Matrix4.CreateTranslation(delta.X * 4f / mGlControl.Width, -delta.Y * 4f / mGlControl.Height, 0f);
            } else {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 三次元データ表示開始
        /// 視点の設定をする
        /// 使い方例
        ///     priave void renderFrame()
        ///     {
        ///         renderFrameStart();
        ///         objectの描画
        ///         renderFrameEnd();
        ///      }
        /// </summary>
        public void renderFrameStart()
        {
            GL.ClearColor(mBackColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //  視界の設定
            Matrix4 modelView = Matrix4.LookAt(Vector3.UnitZ * 10 / mZoom, Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelView);
            GL.MultMatrix(ref mRotate);
            //  視体積の設定
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4 / mZoom, mGlControl.Width / mGlControl.Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        /// <summary>
        /// 三次元データ表示終了
        /// バッファをスワップして表示
        /// </summary>
        public void rendeFrameEnd()
        {
            mGlControl.SwapBuffers();
        }

        /// <summary>
        /// 軸の表示
        /// </summary>
        public void drawAxis()
        {
            //  軸の表示
            Color color = getBackColor() == Color4.Black ? Color.White : Color.Black;
            GL.Color3(color);
            setFontFace(GL3DLib.FONTFACE.XZ);
            //  原点軸の表示
            if (mAxisDisp) {
                drawLine(new Vector3(mMin.X, 0f, 0f), new Vector3(mMax.X * 1.1f, 0f, 0f));
                drawLine(new Vector3(0f, mMin.Y, 0f), new Vector3(0f, mMax.Y * 1.1f, 0f));
                drawLine(new Vector3(0f, 0f, mMin.Z), new Vector3(0f, 0f, mMax.Z * 1.1f));
                drawChar(new Vector3(mMax.X * 1.1f, 0f, 0f), 'X', mFontSize, color);
                drawChar(new Vector3(0f, mMax.Y * 1.1f, 0f), 'Y', mFontSize, color);
                drawChar(new Vector3(0f, 0f, mMax.Z * 1.1f), 'Z', mFontSize, color);
            }

            //  標示領域の枠表示
            if (mAxisFrameDisp) {
                if (mMin.Z < 0f && 0f < mMax.Z) {
                    //  Z=0平面
                    drawLine(new Vector3(mMin.X, mMin.Y, 0f), new Vector3(mMax.X, mMin.Y, 0f));
                    drawLine(new Vector3(mMin.X, mMax.Y, 0f), new Vector3(mMax.X, mMax.Y, 0f));
                    drawLine(new Vector3(mMin.X, mMin.Y, 0f), new Vector3(mMin.X, mMax.Y, 0f));
                    drawLine(new Vector3(mMax.X, mMin.Y, 0f), new Vector3(mMax.X, mMax.Y, 0f));
                }
                //  Z=max平面
                drawLine(new Vector3(mMin.X, mMin.Y, mMax.Z), new Vector3(mMax.X, mMin.Y, mMax.Z));
                drawLine(new Vector3(mMin.X, mMax.Y, mMax.Z), new Vector3(mMax.X, mMax.Y, mMax.Z));
                drawLine(new Vector3(mMin.X, mMin.Y, mMax.Z), new Vector3(mMin.X, mMax.Y, mMax.Z));
                drawLine(new Vector3(mMax.X, mMin.Y, mMax.Z), new Vector3(mMax.X, mMax.Y, mMax.Z));
                //  Z=min平面
                drawLine(new Vector3(mMin.X, mMin.Y, mMin.Z), new Vector3(mMax.X, mMin.Y, mMin.Z));
                drawLine(new Vector3(mMin.X, mMax.Y, mMin.Z), new Vector3(mMax.X, mMax.Y, mMin.Z));
                drawLine(new Vector3(mMin.X, mMin.Y, mMin.Z), new Vector3(mMin.X, mMax.Y, mMin.Z));
                drawLine(new Vector3(mMax.X, mMin.Y, mMin.Z), new Vector3(mMax.X, mMax.Y, mMin.Z));

                drawLine(new Vector3(mMin.X, mMin.Y, mMin.Z), new Vector3(mMin.X, mMin.Y, mMax.Z));
                drawLine(new Vector3(mMin.X, mMax.Y, mMin.Z), new Vector3(mMin.X, mMax.Y, mMax.Z));
                drawLine(new Vector3(mMax.X, mMin.Y, mMin.Z), new Vector3(mMax.X, mMin.Y, mMax.Z));
                drawLine(new Vector3(mMax.X, mMax.Y, mMin.Z), new Vector3(mMax.X, mMax.Y, mMax.Z));
            }
        }

        /// <summary>
        /// 表示エリアの設定
        /// </summary>
        /// <param name="min">表示エリアの最小値</param>
        /// <param name="max">表示エリアの最大値</param>
        public void setArea(Vector3 min, Vector3 max)
        {
            mMin = min;
            mMax = max;

            mDis.X = mMax.X - mMin.X;
            mDis.Y = mMax.Y - mMin.Y;
            mDis.Z = mMax.Z - mMin.Z;
        }

        /// <summary>
        /// 表示エリアの拡張
        /// </summary>
        /// <param name="pos">拡張する座標</param>
        public void extendArea(Vector3 pos)
        {
            mMin.X = Math.Min(mMin.X, pos.X);
            mMin.Y = Math.Min(mMin.Y, pos.Y);
            mMin.Z = Math.Min(mMin.Z, pos.Z);
            mMax.X = Math.Max(mMax.X, pos.X);
            mMax.Y = Math.Max(mMax.Y, pos.Y);
            mMax.Z = Math.Max(mMax.Z, pos.Z);

            mDis.X = mMax.X - mMin.X;
            mDis.Y = mMax.Y - mMin.Y;
            mDis.Z = mMax.Z - mMin.Z;
        }

        /// <summary>
        /// 表示エリアを確認してminとmaxが同じ場合±1のエリアを確保する
        /// </summary>
        public void areaCheck()
        {
            if (mMin.X == mMax.X) {
                mMin.X -= 1f;
                mMax.X += 1f;
            }
            if (mMin.Y == mMax.Y) {
                mMin.Y -= 1f;
                mMax.Y += 1f;
            }
            if (mMin.Z == mMax.Z) {
                mMin.Z -= 1f;
                mMax.Z += 1f;
            }
        }

        /// <summary>
        /// 表示エリアの最小座標を取得
        /// </summary>
        /// <returns>座標値</returns>
        public Vector3 getAreaMin()
        {
            return mMin;
        }

        /// <summary>
        /// 表示エリアの最大座標を取得
        /// </summary>
        /// <returns>座標値</returns>
        public Vector3 getAreaMax()
        {
            return mMax;
        }

        /// <summary>
        /// グラフ表示でZ方向の値で色が変化させるためのmin/maxを設定する
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void setColorLevel(float min, float max)
        {
            mColorMin = (double)min;
            mColorMax = (double)max;
        }

        /// <summary>
        /// SurFaceModelとWireFrameModelとの切り替え
        /// </summary>
        /// <param name="surface"></param>
        public void setSurfaceModel(bool surface)
        {
            mIsSurfaceModel = surface;
        }

        /// <summary>
        /// 表示領域の枠表示
        /// </summary>
        /// <param name="disp"></param>
        public void setAreaFrameDisp(bool disp)
        {
            mAxisFrameDisp = disp;
        }

        /// <summary>
        /// 背景色の設定
        /// </summary>
        /// <param name="backColor"></param>
        public void setBackColor(Color4 backColor)
        {
            mBackColor = backColor;
        }

        /// <summary>
        /// 背景色の取得
        /// </summary>
        /// <returns></returns>
        public Color4 getBackColor()
        {
            return mBackColor;
        }

        /// <summary>
        /// 文字サイズの設定
        /// </summary>
        /// <param name="size"></param>
        public void setFontSize(float size)
        {
            mFontSize = size;
        }

        /// <summary>
        /// 文字サイズの取得
        /// </summary>
        /// <returns></returns>
        public float getFontSize()
        {
            return mFontSize;
        }

        /// <summary>
        /// データ領域に納まるスケールに座標データを変換する
        /// デフォルトの領域はX,Y,Zとも±1の範囲
        /// </summary>
        /// <param name="pos">変換前座標</param>
        /// <returns>変換後の座標</returns>
        private Vector3 cnvPosition(Vector3 pos)
        {
            Vector3 p = new Vector3();
            p.X = (pos.X - mMin.X) * 2.0f / mDis.X - 1f;
            p.Y = (pos.Y - mMin.Y) * 2.0f / mDis.Y - 1f;
            p.Z = (pos.Z - mMin.Z) * 2.0f / mDis.Z - 1f;

            return p;
        }


        private Vector3 reCnvPosition(Vector3 pos)
        {
            Vector3 p = new Vector3();
            p.X = (pos.X + 1f) * mDis.X / 2.0f + mMin.X;
            p.Y = (pos.Y + 1f) * mDis.Y / 2.0f + mMin.Y;
            p.Z = (pos.Z + 1f) * mDis.Z / 2.0f + mMin.Z;

            return p;
        }

        /// <summary>
        /// ワイヤーフレームで表示
        /// </summary>
        /// <param name="position">三次元座標データ配列</param>
        public void drawWireShape(Vector3[,] position)
        {
            for (int i = 0; i < position.GetLength(0); i++) {
                GL.Begin(PrimitiveType.LineStrip);
                for (int j = 0; j < position.GetLength(1); j++) {
                    if (position[i, j] != null) {
                        GL.Color4(val2color(position[i, j].Z, mColorMin, mColorMax));
                        GL.Vertex3(cnvPosition(position[i, j]));
                    }
                }
                GL.End();
            }
        }

        /// <summary>
        /// サーフェイスモデルで表示
        /// </summary>
        /// <param name="position">三次元座標データ配列</param>
        public void drawSurfaceShape(Vector3[,] position)
        {
            for (int i = 0; i < position.GetLength(0) - 1; i++) {
                GL.Begin(PrimitiveType.QuadStrip);
                for (int j = 0; j < position.GetLength(1); j++) {
                    GL.Color4(val2color(position[i, j].Z, mColorMin, mColorMax));
                    if (position[i, j] != null)
                        GL.Vertex3(cnvPosition(position[i, j]));
                    if (position[i + 1, j] != null)
                        GL.Vertex3(cnvPosition(position[i + 1, j]));
                }
                GL.End();
            }
        }

        /// <summary>
        /// サーフェイスモデルで表示
        /// </summary>
        /// <param name="val2Color">色設定関数(関数ポインタ)</param>
        /// <param name="position">三次元座標データ配列</param>
        public void drawSurfaceShape(Func<double, Color4> val2Color, Vector3[,] position)
        {
            for (int i = 0; i < position.GetLength(0) - 1; i++) {
                GL.Begin(PrimitiveType.QuadStrip);
                for (int j = 0; j < position.GetLength(1); j++) {
                    GL.Color4(val2Color(position[i, j].Z));
                    if (position[i, j] != null)
                        GL.Vertex3(cnvPosition(position[i, j]));
                    if (position[i + 1, j] != null)
                        GL.Vertex3(cnvPosition(position[i + 1, j]));
                }
                GL.End();
            }
        }

        /// <summary>
        /// スケールを考慮した線分の描画
        /// </summary>
        /// <param name="ps">始点</param>
        /// <param name="pe">終点</param>
        public void drawLine(Vector3 ps, Vector3 pe)
        {
            line(cnvPosition(ps), cnvPosition(pe));
        }

        /// <summary>
        /// 線分の描画
        /// </summary>
        /// <param name="ps">始点</param>
        /// <param name="pe">終点</param>
        public void line(Vector3 ps, Vector3 pe)
        {
            line(ps.X, ps.Y, ps.Z, pe.X, pe.Y, pe.Z);
        }

        /// <summary>
        /// 文字を表示する面を指定する
        /// </summary>
        /// <param name="face">表示面</param>
        public void setFontFace(FONTFACE face)
        {
            mFontFace = face;
        }

        /// <summary>
        /// 文字列を表示する
        /// </summary>
        /// <param name="p">起点座標</param>
        /// <param name="text">文字列</param>
        /// <param name="size">文字の大きさ</param>
        /// <param name="color">文字の色</param>
        public void drawText(Vector3 p, string text, float size, Color color)
        {
            //float pitch = (2.5232f / 25f * size) * mDis.X / 2.0f;
            float[] pitch = new float[] { (2.5232f / 25f * size) * mDis.X / 2.0f, 0f, 0f };
            for (int i = 0; i < text.Length; i++) {
                drawChar(p, text[i], size, color);
                p.X += pitch[mFontFaceData[(int)mFontFace, 0]];
                p.Y += pitch[mFontFaceData[(int)mFontFace, 1]];
                p.Z += pitch[mFontFaceData[(int)mFontFace, 2]];
            }
        }

        /// <summary>
        /// ベクトル文字の表示(半角ベクトル文字)
        /// </summary>
        /// <param name="p">座標</param>
        /// <param name="c">文字</param>
        /// <param name="size">大きさ</param>
        /// <param name="color">色</param>
        public void drawChar(Vector3 p, char c, float size, Color color)
        {
            if ('0' <= c && c <= '9') {
                int n = c - '0';
                drawChar(mNumberFont[n], p, size, color);
            } else if ('!' <= c && c <= '/') {
                int n = c - '!';
                drawChar(mMarkFont[n], p, size, color);
            } else if ('A' <= c && c <= 'Z') {
                int n = c - 'A';
                drawChar(mLargeAlphaFont[n], p, size, color);
            } else if ('a' <= c && c <= 'z') {
                int n = c - 'a';
                drawChar(mSmallAlphaFont[n], p, size, color);
            }
        }

        /// <summary>
        /// 文字ベクターデータを標示する
        /// </summary>
        /// <param name="fontVector">文字データの種類</param>
        /// <param name="p">起点座標</param>
        /// <param name="size">文字の大きさ</param>
        /// <param name="color">文字色</param>
        private void drawChar(float[,] fontVector, Vector3 p, float size, Color color)
        {
            float[] vector = new float[fontVector.Length];
            int start = 0;
            p = cnvPosition(p);
            size /= 20f;
            for (int i = 0; i < fontVector.Length; i += 3) {
                if (fontVector[i / 3, 0] < 0) {
                    polyLine(vector, start / 3, (i - start) / 3, color, 2f);
                    start = i + 3;
                } else {
                    vector[i] = fontVector[i / 3, mFontFaceData[(int)mFontFace, 0]] * size + p.X;
                    vector[i + 1] = fontVector[i / 3, mFontFaceData[(int)mFontFace, 1]] * size + p.Y;
                    vector[i + 2] = fontVector[i / 3, mFontFaceData[(int)mFontFace, 2]] * size + p.Z;
                }
            }
            polyLine(vector, start / 3, (vector.Length - start) / 3, color, 2f);
        }

        /// <summary>
        /// 連続線分を描画
        /// </summary>
        /// <param name="array">頂点データ</param>
        /// <param name="start">開始位置(3データづつ)</param>
        /// <param name="size">データサイズ(3データづつ)</param>
        /// <param name="color">色</param>
        /// <param name="width">線の太さ</param>
        public void polyLine(float[] array, int start, int size, Color color, float width)
        {
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color3(color);
            GL.LineWidth(width);
            for (int i = 0; i < size; i++) {
                GL.Vertex3(array[(start + i) * 3], array[(start + i) * 3 + 1], array[(start + i) * 3 + 2]);
            }
            GL.End();
        }

        /// <summary>
        /// 線分の描画
        /// </summary>
        /// <param name="x1">始点X</param>
        /// <param name="y1">始点Y</param>
        /// <param name="z1">始点Z</param>
        /// <param name="x2">終点X</param>
        /// <param name="y2">終点Y</param>
        /// <param name="z2">終点Z</param>
        public void line(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(x1, y1, z1);
            GL.Vertex3(x2, y2, z2);
            GL.End();
        }

        /// <summary>
        /// 破線の描画
        /// </summary>
        /// <param name="x1">始点X</param>
        /// <param name="y1">始点Y</param>
        /// <param name="z1">始点Z</param>
        /// <param name="x2">終点X</param>
        /// <param name="y2">終点Y</param>
        /// <param name="z2">終点Z</param>
        public void dot_line(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            GL.Enable(EnableCap.LineStipple);   //  破線パターン
            GL.LineStipple(1, 0xe0e0);          //  LinePattern=0b1110000011100000=0xe0e0   16ビットパターン設定
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex3(x1, y1, z1);
            GL.Vertex3(x2, y2, z2);
            GL.End();
            GL.Disable(EnableCap.LineStipple);
        }

        /// <summary>
        /// 値に応じて色を求める
        /// </summary>
        /// <param name="val">値</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns></returns>
        public Color4 val2color(double val, double min, double max)
        {
            Color4 col = new Color4();
            float nVal = (float)((val - min) / (max - min));
            if (nVal < 0)
                nVal = 0f;
            if (1.0 < nVal)
                nVal = 1.0f;
            if (nVal < 0.5) {
                col.R = 0.0f;
                col.G = nVal * 2.0f;
                col.B = 1.0f - nVal * 2.0f;
            } else {
                col.B = 0.0f;
                col.R = nVal * 2.0f;
                col.G = 2.0f - nVal * 2.0f;
            }
            return col;
        }

        /// <summary>
        /// 文字データを設定する
        /// </summary>
        private void FontInit()
        {
            mMarkFont[0] = new float[,] {  //  !
				{ 0.75f, 3.80f, 0.0f },
                { 0.75f, 1.90f, 0.0f },
                { -1.0f, -1.0f, 0.0f },
                { 0.75f, 1.20f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
            };
            mMarkFont[1] = new float[,] {  //  "
				{ 0.75f, 3.80f, 0.0f },
                { 0.20f, 3.00f, 0.0f },
                { -1.0f, -1.0f, 0.0f },
                { 1.25f, 3.80f, 0.0f },
                { 0.70f, 3.00f, 0.0f },
            };
            mMarkFont[2] = new float[,] {  //  #
				{ 0.45f, 3.40f, 0.0f },
                { 0.45f, 0.70f, 0.0f },
                { 0.45f, 1.50f, 0.0f },
                { 0.25f, 1.50f, 0.0f },
                { 1.25f, 1.50f, 0.0f },
                { 0.95f, 1.50f, 0.0f },
                { 0.95f, 0.70f, 0.0f },
                { 0.95f, 3.40f, 0.0f },
                { 0.95f, 2.60f, 0.0f },
                { 1.25f, 2.60f, 0.0f },
                { 0.25f, 2.60f, 0.0f },
            };
            mMarkFont[3] = new float[,] {  //  $
				{ 1.25f, 3.40f, 0.0f },
                { 0.45f, 3.40f, 0.0f },
                { 0.25f, 3.00f, 0.0f },
                { 0.25f, 2.65f, 0.0f },
                { 0.45f, 2.25f, 0.0f },
                { 1.00f, 2.25f, 0.0f },
                { 1.25f, 1.90f, 0.0f },
                { 1.25f, 1.50f, 0.0f },
                { 1.00f, 1.15f, 0.0f },
                { 0.25f, 1.15f, 0.0f },
                { 0.75f, 1.15f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
                { 0.75f, 3.80f, 0.0f },
            };
            mMarkFont[4] = new float[,] {  //  %
				{ 0.20f, 3.40f, 0.0f },
                { 0.20f, 2.85f, 0.0f },
                { 0.55f, 2.85f, 0.0f },
                { 0.55f, 3.40f, 0.0f },
                { 0.20f, 3.40f, 0.0f },
                { -1.0f, -1.0f, 0.0f },
                { 1.50f, 3.40f, 0.0f },
                { 0.02f, 1.10f, 0.0f },
                { -1.0f, -1.0f, 0.0f },
                { 0.95f, 1.65f, 0.0f },
                { 0.95f, 1.15f, 0.0f },
                { 1.30f, 1.15f, 0.0f },
                { 1.30f, 1.65f, 0.0f },
                { 0.95f, 1.65f, 0.0f },
            };
            mMarkFont[5] = new float[,] {  //  &
				{ 1.50f, 1.90f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
                { 0.50f, 0.70f, 0.0f },
                { 0.00f, 1.50f, 0.0f },
                { 0.00f, 1.90f, 0.0f },
                { 0.95f, 3.40f, 0.0f },
                { 0.70f, 3.80f, 0.0f },
                { 0.50f, 3.80f, 0.0f },
                { 0.25f, 3.40f, 0.0f },
                { 0.25f, 2.60f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
            };
            mMarkFont[6] = new float[,] {  //  '
				{ 1.00f, 3.80f, 0.0f },
                { 0.45f, 3.00f, 0.0f },
            };
            mMarkFont[7] = new float[,] {  //  (
				{ 1.25f, 3.80f, 0.0f },
                { 1.00f, 3.35f, 0.0f },
                { 0.75f, 2.70f, 0.0f },
                { 0.75f, 1.90f, 0.0f },
                { 1.00f, 1.20f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
            };
            mMarkFont[8] = new float[,] {  //  )
				{ 0.25f, 3.80f, 0.0f },
                { 0.50f, 3.35f, 0.0f },
                { 0.75f, 2.60f, 0.0f },
                { 0.75f, 1.90f, 0.0f },
                { 0.45f, 1.15f, 0.0f },
                { 0.20f, 0.70f, 0.0f },
            };
            mMarkFont[9] = new float[,] {  //  *
				{ 0.75f, 3.80f, 0.0f },
                { 0.75f, 2.25f, 0.0f },
                { 0.25f, 3.00f, 0.0f },
                { 1.30f, 1.45f, 0.0f },
                { 0.75f, 2.25f, 0.0f },
                { 0.25f, 1.45f, 0.0f },
                { 1.25f, 3.00f, 0.0f },
                { 0.75f, 2.25f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
            };
            mMarkFont[10] = new float[,] {  //  +
				{ 0.75f, 3.40f, 0.0f },
                { 0.75f, 1.10f, 0.0f },
                { 0.75f, 2.25f, 0.0f },
                { 0.25f, 2.25f, 0.0f },
                { 1.25f, 2.25f, 0.0f },
            };
            mMarkFont[11] = new float[,] {  //  ,
				{ 0.75f, 0.70f, 0.0f },
                { 0.45f, 0.70f, 0.0f },
                { 0.45f, 1.10f, 0.0f },
                { 0.75f, 1.10f, 0.0f },
                { 0.75f, 0.50f, 0.0f },
                { 0.45f, 0.10f, 0.0f },
            };
            mMarkFont[12] = new float[,] {  //  -
				{ 0.25f, 2.25f, 0.0f },
                { 1.25f, 2.25f, 0.0f },
            };
            mMarkFont[13] = new float[,] {  //  .
				{ 0.45f, 1.10f, 0.0f },
                { 0.45f, 0.70f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
                { 0.75f, 1.10f, 0.0f },
                { 0.45f, 1.10f, 0.0f },
            };
            mMarkFont[14] = new float[,] {  //  /
				{ 1.50f, 3.80f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
            };
            mNumberFont[0] = new float[,] {  //  0
                { 0.95f, 3.80f, 0.0f },
                { 0.25f, 3.80f, 0.0f },
                { 0.00f, 3.00f, 0.0f },
                { 0.00f, 1.50f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 1.00f, 0.70f, 0.0f },
                { 1.25f, 1.50f, 0.0f },
                { 1.25f, 3.00f, 0.0f },
                { 0.95f, 3.80f, 0.0f },
            };
            mNumberFont[1] = new float[,] {  //  1
				{ 0.45f, 3.40f, 0.0f },
                { 0.75f, 3.80f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
                { 0.45f, 0.70f, 0.0f },
                { 0.95f, 0.70f, 0.0f },
            };
            mNumberFont[2] = new float[,] {  //  2
				{ 0.00f, 3.35f, 0.0f },
                { 0.25f, 3.80f, 0.0f },
                { 0.95f, 3.80f, 0.0f },
                { 1.25f, 3.35f, 0.0f },
                { 1.25f, 3.00f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
            };
            mNumberFont[3] = new float[,] {  //  3
 				{ 0.00f, 3.80f, 0.0f },
                { 0.95f, 3.80f, 0.0f },
                { 1.25f, 3.40f, 0.0f },
                { 1.25f, 3.00f, 0.0f },
                { 0.75f, 2.25f, 0.0f },
                { 0.50f, 2.25f, 0.0f },
                { 0.75f, 2.25f, 0.0f },
                { 1.25f, 1.50f, 0.0f },
                { 1.25f, 1.10f, 0.0f },
                { 0.95f, 0.70f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 0.00f, 1.10f, 0.0f },
           };
            mNumberFont[4] = new float[,] {  //  4
				{ 0.50f, 3.80f, 0.0f },
                { 0.00f, 1.50f, 0.0f },
                { 1.30f, 1.50f, 0.0f },
                { 0.90f, 1.50f, 0.0f },
                { 0.90f, 2.60f, 0.0f },
                { 0.90f, 0.70f, 0.0f },
            };
            mNumberFont[5] = new float[,] {  //  5
				{ 1.25f, 3.80f, 0.0f },
                { 0.00f, 3.80f, 0.0f },
                { 0.00f, 2.25f, 0.0f },
                { 0.95f, 2.25f, 0.0f },
                { 1.25f, 1.90f, 0.0f },
                { 1.25f, 1.15f, 0.0f },
                { 0.95f, 0.70f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
            };
            mNumberFont[6] = new float[,] {  //  6
				{ 1.25f, 3.35f, 0.0f },
                { 0.95f, 3.80f, 0.0f },
                { 0.25f, 3.80f, 0.0f },
                { 0.00f, 3.35f, 0.0f },
                { 0.00f, 1.10f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 0.95f, 0.70f, 0.0f },
                { 1.25f, 1.10f, 0.0f },
                { 1.25f, 1.90f, 0.0f },
                { 0.95f, 2.25f, 0.0f },
                { 0.00f, 2.25f, 0.0f },
            };
            mNumberFont[7] = new float[,] {  //  7
				{ 0.00f, 3.80f, 0.0f },
                { 1.25f, 3.80f, 0.0f },
                { 1.25f, 3.40f, 0.0f },
                { 0.95f, 1.50f, 0.0f },
                { 0.95f, 0.70f, 0.0f },
            };
            mNumberFont[8] = new float[,] {  //  8
				{ 1.00f, 2.25f, 0.0f },
                { 1.25f, 2.60f, 0.0f },
                { 1.25f, 3.40f, 0.0f },
                { 1.00f, 3.80f, 0.0f },
                { 0.25f, 3.80f, 0.0f },
                { 0.00f, 3.40f, 0.0f },
                { 0.00f, 2.60f, 0.0f },
                { 0.25f, 2.25f, 0.0f },
                { 1.00f, 2.25f, 0.0f },
                { 1.25f, 1.90f, 0.0f },
                { 1.25f, 1.15f, 0.0f },
                { 1.00f, 0.70f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.00f, 1.90f, 0.0f },
                { 0.25f, 2.25f, 0.0f },
            };
            mNumberFont[9] = new float[,] {  //  9
				{ 1.25f, 2.25f, 0.0f },
                { 0.25f, 2.25f, 0.0f },
                { 0.00f, 2.60f, 0.0f },
                { 0.00f, 3.40f, 0.0f },
                { 0.25f, 3.80f, 0.0f },
                { 1.00f, 3.80f, 0.0f },
                { 1.25f, 3.40f, 0.0f },
                { 1.25f, 1.15f, 0.0f },
                { 1.00f, 0.70f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
            };

            mLargeAlphaFont[0] = new float[,] { //  A
				{ 0.00f, 0.70f, 0.0f },
                { 0.00f, 3.00f, 0.0f },
                { 0.50f, 3.80f, 0.0f },
                { 1.00f, 3.80f, 0.0f },
                { 1.50f, 3.00f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
                { 1.50f, 2.25f, 0.0f },
                { 0.00f, 2.25f, 0.0f },
            };
            mLargeAlphaFont[1] = new float[,] { //  B
				{ 0.00f, 3.80f, 0.0f },
                { 1.25f, 3.80f, 0.0f },
                { 1.50f, 3.40f, 0.0f },
                { 1.50f, 2.60f, 0.0f },
                { 1.25f, 2.25f, 0.0f },
                { 0.00f, 2.25f, 0.0f },
                { 1.25f, 2.25f, 0.0f },
                { 1.50f, 1.90f, 0.0f },
                { 1.50f, 1.15f, 0.0f },
                { 1.50f, 1.15f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 0.00f, 3.80f, 0.0f },
            };
            mLargeAlphaFont[2] = new float[,] { //  C
				{ 1.50f, 3.80f, 0.0f },
                { 0.25f, 3.80f, 0.0f },
                { 0.00f, 3.35f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
            };
            mLargeAlphaFont[3] = new float[,] { //  D
				{ 0.00f, 3.80f, 0.0f },
                { 1.25f, 3.80f, 0.0f },
                { 1.50f, 3.35f, 0.0f },
                { 1.50f, 1.15f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 0.00f, 3.80f, 0.0f },
            };
            mLargeAlphaFont[4] = new float[,] { //  E
				{ 1.50f, 3.80f, 0.0f },
                { 0.00f, 3.80f, 0.0f },
                { 0.00f, 2.25f, 0.0f },
                { 1.25f, 2.25f, 0.0f },
                { 0.00f, 2.25f, 0.0f },
                { 0.00f, 0.75f, 0.0f },
                { 1.50f, 0.75f, 0.0f },
            };
            mLargeAlphaFont[5] = new float[,] { //  F
				{ 1.50f, 3.80f, 0.0f },
                { 0.00f, 3.80f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 0.00f, 2.25f, 0.0f },
                { 1.25f, 2.25f, 0.0f },
            };
            mLargeAlphaFont[6] = new float[,] { //  G
				{ 1.50f, 3.80f, 0.0f },
                { 0.25f, 3.80f, 0.0f },
                { 0.00f, 3.40f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
                { 1.50f, 1.10f, 0.0f },
                { 1.50f, 2.25f, 0.0f },
                { 0.75f, 2.25f, 0.0f },
            };
            mLargeAlphaFont[7] = new float[,] { //  H
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 0.00f, 2.25f, 0.0f },
                { 1.50f, 2.25f, 0.0f },
                { 1.50f, 3.80f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
            };
            mLargeAlphaFont[8] = new float[,] { //  I
				{ 0.75f, 3.80f, 0.0f },
                { 1.25f, 3.80f, 0.0f },
                { 1.00f, 3.80f, 0.0f },
                { 1.00f, 0.70f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
            };
            mLargeAlphaFont[9] = new float[,] { //  J
				{ 1.25f, 3.80f, 0.0f },
                { 1.25f, 1.10f, 0.0f },
                { 1.00f, 0.70f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
            };
            mLargeAlphaFont[10] = new float[,] { //  K
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 0.00f, 2.25f, 0.0f },
                { 0.25f, 2.25f, 0.0f },
                { 1.50f, 3.80f, 0.0f },
                { 0.25f, 2.25f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
            };
            mLargeAlphaFont[11] = new float[,] { //  L
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
            };
            mLargeAlphaFont[12] = new float[,] { //  M
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 0.00f, 3.35f, 0.0f },
                { 0.75f, 2.25f, 0.0f },
                { 0.75f, 1.45f, 0.0f },
                { 0.75f, 2.25f, 0.0f },
                { 1.50f, 3.35f, 0.0f },
                { 1.50f, 3.80f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
            };
            mLargeAlphaFont[13] = new float[,] { //  N
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 0.00f, 3.40f, 0.0f },
                { 1.50f, 1.15f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
                { 1.50f, 3.80f, 0.0f },
            };
            mLargeAlphaFont[14] = new float[,] { //  O
				{ 1.25f, 3.80f, 0.0f },
                { 0.25f, 3.80f, 0.0f },
                { 0.00f, 3.35f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
                { 1.50f, 1.15f, 0.0f },
                { 1.50f, 3.40f, 0.0f },
                { 1.25f, 3.80f, 0.0f },
            };
            mLargeAlphaFont[15] = new float[,] { //  P
				{ 0.00f, 0.70f, 0.0f },
                { 0.00f, 3.80f, 0.0f },
                { 1.25f, 3.80f, 0.0f },
                { 1.50f, 3.40f, 0.0f },
                { 1.50f, 2.60f, 0.0f },
                { 1.25f, 2.25f, 0.0f },
                { 0.00f, 2.25f, 0.0f },
            };
            mLargeAlphaFont[16] = new float[,] { //  Q
				{ 1.35f, 0.90f, 0.0f },
                { 1.50f, 1.15f, 0.0f },
                { 1.50f, 3.40f, 0.0f },
                { 1.25f, 3.80f, 0.0f },
                { 0.25f, 3.80f, 0.0f },
                { 0.00f, 3.40f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
                { 1.35f, 0.90f, 0.0f },
                { 0.75f, 1.90f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
            };
            mLargeAlphaFont[17] = new float[,] { //  R
				{ 0.00f, 0.70f, 0.0f },
                { 0.00f, 3.80f, 0.0f },
                { 1.25f, 3.80f, 0.0f },
                { 1.50f, 3.40f, 0.0f },
                { 1.50f, 2.60f, 0.0f },
                { 1.25f, 2.25f, 0.0f },
                { 0.00f, 2.25f, 0.0f },
                { 0.50f, 2.25f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
            };
            mLargeAlphaFont[18] = new float[,] { //  S
				{ 1.50f, 3.80f, 0.0f },
                { 0.25f, 3.80f, 0.0f },
                { 0.00f, 3.40f, 0.0f },
                { 0.00f, 2.60f, 0.0f },
                { 0.25f, 2.25f, 0.0f },
                { 1.25f, 2.25f, 0.0f },
                { 1.50f, 1.90f, 0.0f },
                { 1.50f, 1.10f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
            };
            mLargeAlphaFont[19] = new float[,] { //  T
				{ 0.00f, 3.80f, 0.0f },
                { 1.50f, 3.80f, 0.0f },
                { 0.75f, 3.80f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
            };
            mLargeAlphaFont[20] = new float[,] { //  U
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
                { 1.50f, 1.10f, 0.0f },
                { 1.50f, 3.80f, 0.0f },
            };
            mLargeAlphaFont[21] = new float[,] { //  V
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 1.90f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
                { 1.50f, 1.90f, 0.0f },
                { 1.50f, 3.80f, 0.0f },
            };
            mLargeAlphaFont[22] = new float[,] { //  W
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 1.35f, 0.0f },
                { 0.40f, 0.70f, 0.0f },
                { 0.80f, 1.35f, 0.0f },
                { 0.80f, 2.60f, 0.0f },
                { 0.80f, 1.35f, 0.0f },
                { 1.15f, 0.70f, 0.0f },
                { 1.60f, 1.35f, 0.0f },
                { 1.60f, 3.80f, 0.0f },
            };
            mLargeAlphaFont[23] = new float[,] { //  X
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 3.40f, 0.0f },
                { 1.50f, 1.15f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
                { 1.50f, 1.15f, 0.0f },
                { 0.75f, 2.30f, 0.0f },
                { 1.50f, 3.40f, 0.0f },
                { 1.50f, 3.80f, 0.0f },
                { 1.50f, 3.40f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
            };
            mLargeAlphaFont[24] = new float[,] { //  Y
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 3.40f, 0.0f },
                { 0.75f, 2.25f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
                { 0.75f, 2.25f, 0.0f },
                { 1.50f, 3.40f, 0.0f },
                { 1.50f, 3.80f, 0.0f },
            };
            mLargeAlphaFont[25] = new float[,] { //  Z
				{ 0.00f, 3.80f, 0.0f },
                { 1.50f, 3.80f, 0.0f },
                { 1.50f, 3.40f, 0.0f },
                { 0.00f, 1.10f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
            };

            mSmallAlphaFont[0] = new float[,] { //  a
				{ 0.00f, 3.00f, 0.0f },
                { 0.95f, 3.00f, 0.0f },
                { 1.25f, 2.60f, 0.0f },
                { 1.25f, 1.15f, 0.0f },
                { 0.95f, 0.70f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.00f, 1.90f, 0.0f },
                { 1.25f, 1.90f, 0.0f },
                { 1.25f, 1.15f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[1] = new float[,] { //  b
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 0.95f, 0.70f, 0.0f },
                { 1.25f, 1.15f, 0.0f },
                { 1.25f, 2.30f, 0.0f },
                { 0.95f, 2.75f, 0.0f },
                { 0.00f, 2.75f, 0.0f },
            };
            mSmallAlphaFont[2] = new float[,] { //  c
				{ 1.25f, 3.00f, 0.0f },
                { 0.20f, 3.00f, 0.0f },
                { 0.00f, 2.60f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.20f, 0.70f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[3] = new float[,] { //  d
				{ 1.25f, 2.75f, 0.0f },
                { 0.25f, 2.75f, 0.0f },
                { 0.00f, 2.30f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
                { 1.25f, 3.80f, 0.0f },
            };
            mSmallAlphaFont[4] = new float[,] { //  e
				{ 0.00f, 1.90f, 0.0f },
                { 1.25f, 1.90f, 0.0f },
                { 1.25f, 2.60f, 0.0f },
                { 1.00f, 3.00f, 0.0f },
                { 0.25f, 3.00f, 0.0f },
                { 0.00f, 2.60f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.25f, 0.70f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[5] = new float[,] { //  f
				{ 0.85f, 3.80f, 0.0f },
                { 0.60f, 3.40f, 0.0f },
                { 0.60f, 0.70f, 0.0f },
                { 0.60f, 2.60f, 0.0f },
                { 0.00f, 2.60f, 0.0f },
                { 1.15f, 2.60f, 0.0f },
            };
            mSmallAlphaFont[6] = new float[,] { //  g
				{ 1.25f, 1.50f, 0.0f },
                { 0.25f, 1.50f, 0.0f },
                { 0.00f, 1.90f, 0.0f },
                { 0.00f, 2.60f, 0.0f },
                { 0.25f, 3.00f, 0.0f },
                { 1.25f, 3.00f, 0.0f },
                { 1.25f, 0.65f, 0.0f },
                { 1.00f, 0.30f, 0.0f },
                { 0.00f, 0.30f, 0.0f },
            };
            mSmallAlphaFont[7] = new float[,] { //  h
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 0.00f, 2.60f, 0.0f },
                { 0.95f, 2.60f, 0.0f },
                { 1.25f, 2.25f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[8] = new float[,] { //  i
				{ 0.75f, 3.80f, 0.0f },
                { 0.75f, 3.35f, 0.0f },
                { -1.0f, -1.0f, 0.0f },
                { 0.75f, 2.60f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[9] = new float[,] { //  j
				{ 1.25f, 3.80f, 0.0f },
                { 1.25f, 3.35f, 0.0f },
                { -1.0f, -1.0f, 0.0f },
                { 1.25f, 2.60f, 0.0f },
                { 1.25f, 0.65f, 0.0f },
                { 1.00f, 0.30f, 0.0f },
                { 0.25f, 0.30f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[10] = new float[,] { //  k
				{ 0.00f, 3.80f, 0.0f },
                { 0.00f, 0.65f, 0.0f },
                { 0.00f, 1.45f, 0.0f },
                { 0.95f, 3.00f, 0.0f },
                { 0.25f, 1.85f, 0.0f },
                { 0.95f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[11] = new float[,] { //  l
				{ 0.75f, 3.80f, 0.0f },
                { 0.75f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[12] = new float[,] { //  m
				{ 0.00f, 3.15f, 0.0f },
                { 0.20f, 3.00f, 0.0f },
                { 0.20f, 0.70f, 0.0f },
                { 0.20f, 3.00f, 0.0f },
                { 0.85f, 3.00f, 0.0f },
                { 0.85f, 0.70f, 0.0f },
                { 0.85f, 3.00f, 0.0f },
                { 1.25f, 3.00f, 0.0f },
                { 1.50f, 2.65f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[13] = new float[,] { //  n
				{ 0.00f, 3.15f, 0.0f },
                { 0.20f, 3.00f, 0.0f },
                { 0.20f, 0.70f, 0.0f },
                { 0.20f, 3.00f, 0.0f },
                { 1.00f, 3.00f, 0.0f },
                { 1.25f, 2.65f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[14] = new float[,] { //  o
				{ 1.00f, 3.00f, 0.0f },
                { 0.20f, 3.00f, 0.0f },
                { 0.00f, 2.60f, 0.0f },
                { 0.00f, 1.10f, 0.0f },
                { 0.20f, 0.70f, 0.0f },
                { 1.00f, 0.70f, 0.0f },
                { 1.25f, 1.10f, 0.0f },
                { 1.25f, 2.60f, 0.0f },
                { 1.00f, 3.00f, 0.0f },
            };
            mSmallAlphaFont[15] = new float[,] { //  p
				{ 0.00f, 1.40f, 0.0f },
                { 0.95f, 1.40f, 0.0f },
                { 1.25f, 1.75f, 0.0f },
                { 1.25f, 2.60f, 0.0f },
                { 0.95f, 3.00f, 0.0f },
                { 0.00f, 3.00f, 0.0f },
                { 0.00f, 0.30f, 0.0f },
            };
            mSmallAlphaFont[16] = new float[,] { //  q
				{ 1.25f, 1.45f, 0.0f },
                { 0.25f, 1.45f, 0.0f },
                { 0.00f, 1.80f, 0.0f },
                { 0.00f, 2.60f, 0.0f },
                { 0.25f, 3.00f, 0.0f },
                { 1.25f, 3.00f, 0.0f },
                { 1.25f, 0.30f, 0.0f },
            };
            mSmallAlphaFont[17] = new float[,] { //  r
				{ 0.00f, 3.05f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 0.00f, 1.90f, 0.0f },
                { 0.75f, 3.05f, 0.0f },
                { 0.95f, 3.05f, 0.0f },
                { 1.25f, 2.60f, 0.0f },
            };
            mSmallAlphaFont[18] = new float[,] { //  s
				{ 1.25f, 3.00f, 0.0f },
                { 0.35f, 3.00f, 0.0f },
                { 0.00f, 2.50f, 0.0f },
                { 0.35f, 1.90f, 0.0f },
                { 0.80f, 1.90f, 0.0f },
                { 1.25f, 1.25f, 0.0f },
                { 0.90f, 0.70f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[19] = new float[,] { //  t
				{ 0.10f, 2.60f, 0.0f },
                { 1.15f, 2.60f, 0.0f },
                { 0.60f, 2.60f, 0.0f },
                { 0.60f, 3.80f, 0.0f },
                { 0.60f, 1.10f, 0.0f },
                { 0.85f, 0.70f, 0.0f },
                { 1.30f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[20] = new float[,] { //  u
				{ 0.00f, 3.00f, 0.0f },
                { 0.00f, 1.15f, 0.0f },
                { 0.20f, 0.70f, 0.0f },
                { 1.00f, 0.70f, 0.0f },
                { 1.25f, 1.15f, 0.0f },
                { 1.50f, 0.70f, 0.0f },
                { 1.25f, 1.15f, 0.0f },
                { 1.25f, 3.00f, 0.0f },
            };
            mSmallAlphaFont[21] = new float[,] { //  v
				{ 0.00f, 3.00f, 0.0f },
                { 0.00f, 1.65f, 0.0f },
                { 0.60f, 0.70f, 0.0f },
                { 1.20f, 1.65f, 0.0f },
                { 1.20f, 3.00f, 0.0f },
            };
            mSmallAlphaFont[22] = new float[,] { //  w
				{ 0.00f, 3.00f, 0.0f },
                { 0.00f, 1.35f, 0.0f },
                { 0.40f, 0.70f, 0.0f },
                { 0.65f, 1.20f, 0.0f },
                { 0.65f, 1.90f, 0.0f },
                { 0.65f, 1.20f, 0.0f },
                { 1.00f, 0.70f, 0.0f },
                { 1.40f, 1.40f, 0.0f },
                { 1.35f, 3.00f, 0.0f },
            };
            mSmallAlphaFont[23] = new float[,] { //  x
				{ 0.00f, 3.00f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
                { 0.60f, 1.90f, 0.0f },
                { 1.25f, 3.00f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
            };
            mSmallAlphaFont[24] = new float[,] { //  y
				{ 0.00f, 3.00f, 0.0f },
                { 0.00f, 1.90f, 0.0f },
                { 0.60f, 0.90f, 0.0f },
                { 0.20f, 0.30f, 0.0f },
                { 1.30f, 1.90f, 0.0f },
                { 1.30f, 3.00f, 0.0f },
            };
            mSmallAlphaFont[25] = new float[,] { //  z
				{ 0.00f, 3.00f, 0.0f },
                { 1.25f, 3.00f, 0.0f },
                { 0.00f, 0.70f, 0.0f },
                { 1.25f, 0.70f, 0.0f },
            };
        }
    }
}
