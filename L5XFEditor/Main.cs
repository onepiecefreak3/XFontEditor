using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using L5XFEditor.Format.FNT;

namespace L5XFEditor
{
    public partial class Main : Form
    {
        Format.XF font = null;

        bool _fileOpened = false;

        #region Init
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("./font.xf"))
            {
                font = new Format.XF(File.OpenRead("./font.xf"));
                _fileOpened = true;

                LoadDataGrid();

                LoadImages();
                DrawCharInfo();

                UpdateUI();
            }
        }
        #endregion

        #region ToolTipMenu
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                OpenFile(fd.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sd = new SaveFileDialog();

            if (sd.ShowDialog() == DialogResult.OK)
            {
                font.Save(File.Create(sd.FileName));

                MessageBox.Show($"Font was successfully saved to {sd.FileName}.", "Font saved.", MessageBoxButtons.OK);
            }
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new FolderBrowserDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                //Extract images
                font.Images[0].Save(Path.Combine(fd.SelectedPath, "font_image0.png"));
                font.Images[1].Save(Path.Combine(fd.SelectedPath, "font_image1.png"));
                font.Images[2].Save(Path.Combine(fd.SelectedPath, "font_image2.png"));
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new FolderBrowserDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                //Import images
                if (File.Exists(Path.Combine(fd.SelectedPath, "font_image0.png")))
                    font.Images[0] = new Bitmap(Path.Combine(fd.SelectedPath, "font_image0.png"));
                if (File.Exists(Path.Combine(fd.SelectedPath, "font_image1.png")))
                    font.Images[1] = new Bitmap(Path.Combine(fd.SelectedPath, "font_image1.png"));
                if (File.Exists(Path.Combine(fd.SelectedPath, "font_image2.png")))
                    font.Images[2] = new Bitmap(Path.Combine(fd.SelectedPath, "font_image2.png"));

                LoadImages();
                DrawCharInfo();
            }
        }
        #endregion

        #region Load
        void LoadDataGrid()
        {
            largeDict.CellValueChanged -= largeDict_CellValueChanged;
            smallDict.CellValueChanged -= smallDict_CellValueChanged;

            largeDict.Rows.Clear();
            smallDict.Rows.Clear();

            int id = 0;
            foreach (var character in font.GlyphsLarge)
            {
                ushort codePoint;
                int colorChannel;
                int imageX, imageY;
                int glyphWidth, glyphHeight;
                int charWidth;
                int offsetX;

                if (character.Value is XF01CharacterMap xf01CharMap)
                {
                    codePoint = character.Key;
                    colorChannel = xf01CharMap.ColorChannel;
                    imageX = xf01CharMap.ImageOffsetX;
                    imageY = xf01CharMap.ImageOffsetY;
                    glyphWidth = ((XF01CharSizeInfo)font.CharSizeInfosLarge[character.Key]).char_width;
                    glyphHeight = ((XF01CharSizeInfo)font.CharSizeInfosLarge[character.Key]).char_height;
                    charWidth = xf01CharMap.CharWidth;
                    offsetX = ((XF01CharSizeInfo)font.CharSizeInfosLarge[character.Key]).offset_x;
                }
                else
                {
                    codePoint = character.Key;
                    colorChannel = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).ColorChannel;
                    imageX = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).ImageOffsetX;
                    imageY = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).ImageOffsetY;
                    glyphWidth = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).char_width;
                    glyphHeight = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).char_height;
                    charWidth = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).CharWidth;
                    offsetX = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).offset_x;
                }

                largeDict.Rows.Add(
                    (char)codePoint,
                    colorChannel,
                    imageX,
                    imageY,
                    glyphWidth,
                    glyphHeight,
                    charWidth,
                    offsetX);
                largeDict.Rows[id].HeaderCell.Value = String.Format("{0}", id++);
            }

            id = 0;
            foreach (var character in font.GlyphsSmall)
            {
                ushort codePoint;
                int colorChannel;
                int imageX, imageY;
                int glyphWidth, glyphHeight;
                int charWidth;
                int offsetX;

                if (character.Value is XF01CharacterMap xf01CharMap)
                {
                    codePoint = character.Key;
                    colorChannel = xf01CharMap.ColorChannel;
                    imageX = xf01CharMap.ImageOffsetX;
                    imageY = xf01CharMap.ImageOffsetY;
                    glyphWidth = ((XF01CharSizeInfo)font.CharSizeInfosSmall[character.Key]).char_width;
                    glyphHeight = ((XF01CharSizeInfo)font.CharSizeInfosSmall[character.Key]).char_height;
                    charWidth = xf01CharMap.CharWidth;
                    offsetX = ((XF01CharSizeInfo)font.CharSizeInfosSmall[character.Key]).offset_x;
                }
                else
                {
                    codePoint = character.Key;
                    colorChannel = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).ColorChannel;
                    imageX = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).ImageOffsetX;
                    imageY = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).ImageOffsetY;
                    glyphWidth = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).char_width;
                    glyphHeight = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).char_height;
                    charWidth = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).CharWidth;
                    offsetX = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).offset_x;
                }

                smallDict.Rows.Add(
                    (char)codePoint,
                    colorChannel,
                    imageX,
                    imageY,
                    glyphWidth,
                    glyphHeight,
                    charWidth,
                    offsetX);
                smallDict.Rows[id].HeaderCell.Value = String.Format("{0}", id++);
            }

            largeDict.CellValueChanged += largeDict_CellValueChanged;
            smallDict.CellValueChanged += smallDict_CellValueChanged;
        }

        void LoadImages()
        {
            image0.Image = new Bitmap(font.Images[0].Width, font.Images[0].Height);
            image1.Image = new Bitmap(font.Images[1].Width, font.Images[1].Height);
            image2.Image = new Bitmap(font.Images[2].Width, font.Images[2].Height);

            Graphics g = Graphics.FromImage(image0.Image);
            g.DrawImage(font.Images[0], new PointF(0, 0));

            g = Graphics.FromImage(image1.Image);
            g.DrawImage(font.Images[1], new PointF(0, 0));

            g = Graphics.FromImage(image2.Image);
            g.DrawImage(font.Images[2], new PointF(0, 0));
        }

        void DrawCharInfo()
        {
            int colorChannel;
            int imageX, imageY;
            int glyphWidth, glyphHeight;

            foreach (var character in font.GlyphsLarge)
            {
                if (character.Value is XF01CharacterMap xf01CharMap)
                {
                    colorChannel = xf01CharMap.ColorChannel;
                    imageX = xf01CharMap.ImageOffsetX;
                    imageY = xf01CharMap.ImageOffsetY;
                    glyphWidth = ((XF01CharSizeInfo)font.CharSizeInfosLarge[character.Key]).char_width;
                    glyphHeight = ((XF01CharSizeInfo)font.CharSizeInfosLarge[character.Key]).char_height;
                }
                else
                {
                    colorChannel = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).ColorChannel;
                    imageX = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).ImageOffsetX;
                    imageY = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).ImageOffsetY;
                    glyphWidth = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).char_width;
                    glyphHeight = ((XF00CharSizeInfo)font.CharSizeInfosLarge[character.Key]).char_height;
                }

                Graphics g = null;
                switch (colorChannel)
                {
                    case 0:
                        g = Graphics.FromImage(image0.Image);
                        break;
                    case 1:
                        g = Graphics.FromImage(image1.Image);
                        break;
                    case 2:
                        g = Graphics.FromImage(image2.Image);
                        break;
                }

                var c = Color.Red;
                g.DrawRectangle(new Pen(c), new Rectangle(imageX, imageY, glyphWidth, glyphHeight));
            }

            foreach (var character in font.GlyphsSmall)
            {
                if (character.Value is XF01CharacterMap xf01CharMap)
                {
                    colorChannel = xf01CharMap.ColorChannel;
                    imageX = xf01CharMap.ImageOffsetX;
                    imageY = xf01CharMap.ImageOffsetY;
                    glyphWidth = ((XF01CharSizeInfo)font.CharSizeInfosSmall[character.Key]).char_width;
                    glyphHeight = ((XF01CharSizeInfo)font.CharSizeInfosSmall[character.Key]).char_height;
                }
                else
                {
                    colorChannel = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).ColorChannel;
                    imageX = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).ImageOffsetX;
                    imageY = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).ImageOffsetY;
                    glyphWidth = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).char_width;
                    glyphHeight = ((XF00CharSizeInfo)font.CharSizeInfosSmall[character.Key]).char_height;
                }

                Graphics g = null;
                switch (colorChannel)
                {
                    case 0:
                        g = Graphics.FromImage(image0.Image);
                        break;
                    case 1:
                        g = Graphics.FromImage(image1.Image);
                        break;
                    case 2:
                        g = Graphics.FromImage(image2.Image);
                        break;
                }

                var c = Color.DarkRed;
                g.DrawRectangle(new Pen(c), new Rectangle(imageX, imageY, glyphWidth, glyphHeight));
            }
        }
        #endregion

        void OpenFile(string file)
        {
            font = new Format.XF(File.OpenRead(file));
            _fileOpened = true;

            LoadDataGrid();

            LoadImages();
            DrawCharInfo();

            UpdateUI();
        }

        void UpdateUI()
        {
            (menuTool.Items[0] as ToolStripMenuItem).DropDownItems[1].Enabled = _fileOpened;
            (menuTool.Items[0] as ToolStripMenuItem).DropDownItems[2].Enabled = _fileOpened;
            (menuTool.Items[1] as ToolStripMenuItem).DropDownItems[0].Enabled = _fileOpened;
            (menuTool.Items[1] as ToolStripMenuItem).DropDownItems[1].Enabled = _fileOpened;

            switch (imgSelector.SelectedIndex)
            {
                case 0:
                    image0.Visible = true;
                    image1.Visible = false;
                    image2.Visible = false;
                    break;
                case 1:
                    image0.Visible = false;
                    image1.Visible = true;
                    image2.Visible = false;
                    break;
                case 2:
                    image0.Visible = false;
                    image1.Visible = false;
                    image2.Visible = true;
                    break;
            }
        }

        #region Events
        private void imgSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void largeDict_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var glyphRow = largeDict.Rows[e.RowIndex];

            ushort codePoint = Convert.ToChar(glyphRow.Cells["Character"].Value);
            var origGlyph = font.GlyphsLarge[codePoint];

            int colorChannel = Convert.ToInt32(glyphRow.Cells["ImageID"].Value);
            int imageX = Convert.ToInt32(glyphRow.Cells["X"].Value);
            int imageY = Convert.ToInt32(glyphRow.Cells["Y"].Value);
            int glyphWidth = Convert.ToUInt16(glyphRow.Cells["GlyphWidth"].Value);

            if (origGlyph is XF01CharacterMap xf01CharMap)
            {
                font.GlyphsLarge[codePoint] = new XF01CharacterMap
                {
                    code_point = codePoint,
                    char_size = (ushort)(xf01CharMap.CharSizeInfoIndex | ((glyphWidth & 0x3F) << 10)),
                    image_offset = colorChannel | (imageX << 4) | (imageY << 18)
                };
            }

            var glyphSize = font.CharSizeInfosLarge[codePoint];
            byte charWidth = Convert.ToByte(glyphRow.Cells["Height"].Value);
            byte charHeight = Convert.ToByte(glyphRow.Cells["Width"].Value);
            sbyte offsetX = Convert.ToSByte(glyphRow.Cells["OffsetX"].Value);

            if (glyphSize is XF01CharSizeInfo xf01CharSize)
            {
                font.CharSizeInfosLarge[codePoint] = new XF01CharSizeInfo
                {
                    char_width = charWidth,
                    char_height = charHeight,
                    offset_x = offsetX,
                    offset_y = xf01CharSize.offset_y
                };
            }
            else
            {
                font.CharSizeInfosLarge[codePoint] = new XF00CharSizeInfo
                {
                    char_width = charWidth,
                    char_height = charHeight,
                    offset_x = offsetX,
                    offset_y = ((XF00CharSizeInfo)font.CharSizeInfosLarge[codePoint]).offset_y,
                    image_info = (uint)(((imageY & 0x3FF) << 22) | ((imageX & 0x3FF) << 12) | ((colorChannel & 0xF) << 8) | (glyphWidth & 0xFF))
                };
            }

            LoadImages();
            DrawCharInfo();
        }

        private void smallDict_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var glyphRow = smallDict.Rows[e.RowIndex];

            ushort codePoint = Convert.ToChar(glyphRow.Cells["Character"].Value);
            var origGlyph = font.GlyphsSmall[codePoint];

            int colorChannel = Convert.ToInt32(glyphRow.Cells["ImageID"].Value);
            int imageX = Convert.ToInt32(glyphRow.Cells["X"].Value);
            int imageY = Convert.ToInt32(glyphRow.Cells["Y"].Value);
            int glyphWidth = Convert.ToUInt16(glyphRow.Cells["GlyphWidth"].Value);

            if (origGlyph is XF01CharacterMap xf01CharMap)
            {
                font.GlyphsSmall[codePoint] = new XF01CharacterMap
                {
                    code_point = codePoint,
                    char_size = (ushort)(xf01CharMap.CharSizeInfoIndex | ((glyphWidth & 0x3F) << 10)),
                    image_offset = colorChannel | (imageX << 4) | (imageY << 18)
                };
            }

            var glyphSize = font.CharSizeInfosSmall[codePoint];
            byte charWidth = Convert.ToByte(glyphRow.Cells["Height"].Value);
            byte charHeight = Convert.ToByte(glyphRow.Cells["Width"].Value);
            sbyte offsetX = Convert.ToSByte(glyphRow.Cells["OffsetX"].Value);

            if (glyphSize is XF01CharSizeInfo xf01CharSize)
            {
                font.CharSizeInfosSmall[codePoint] = new XF01CharSizeInfo
                {
                    char_width = charWidth,
                    char_height = charHeight,
                    offset_x = offsetX,
                    offset_y = xf01CharSize.offset_y
                };
            }
            else
            {
                font.CharSizeInfosSmall[codePoint] = new XF00CharSizeInfo
                {
                    char_width = charWidth,
                    char_height = charHeight,
                    offset_x = offsetX,
                    offset_y = ((XF00CharSizeInfo)font.CharSizeInfosSmall[codePoint]).offset_y,
                    image_info = (uint)(((imageY & 0x3FF) << 22) | ((imageX & 0x3FF) << 12) | ((colorChannel & 0xF) << 8) | (glyphWidth & 0xFF))
                };
            }

            LoadImages();
            DrawCharInfo();
        }

        string firstTextboxChar = "";
        private void searchTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (searchTextBox1.Text.Length > 0 && searchTextBox1.Text[0].ToString() != firstTextboxChar)
            {
                firstTextboxChar = searchTextBox1.Text[0].ToString();
                foreach (DataGridViewRow row in largeDict.Rows)
                {
                    if (Convert.ToChar(row.Cells["Character"].Value) == searchTextBox1.Text[0])
                    {
                        largeDict.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
        }

        string firstTextboxChar2 = "";
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (searchTextBox2.Text.Length > 0 && searchTextBox2.Text[0].ToString() != firstTextboxChar2)
            {
                firstTextboxChar2 = searchTextBox2.Text[0].ToString();
                foreach (DataGridViewRow row in smallDict.Rows)
                {
                    if ((char)row.Cells["Character2"].Value == searchTextBox2.Text[0])
                    {
                        smallDict.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
        }

        private void image0_MouseMove(object sender, MouseEventArgs e)
        {
            WriteCoordinates(image0.PointToImage(new Point(e.X, e.Y)));
        }

        private void image1_MouseMove(object sender, MouseEventArgs e)
        {
            WriteCoordinates(image1.PointToImage(new Point(e.X, e.Y)));
        }

        private void image2_MouseMove(object sender, MouseEventArgs e)
        {
            WriteCoordinates(image2.PointToImage(new Point(e.X, e.Y)));
        }

        void WriteCoordinates(Point p)
        {
            coordinatesLabel.Text = "X: " + p.X + ", Y: " + p.Y;
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            OpenFile(files[0]);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fileOpened = false;

            font.Dispose();
            font = null;

            largeDict.Rows.Clear();
            smallDict.Rows.Clear();

            image0.Image.Dispose();
            image1.Image.Dispose();
            image2.Image.Dispose();

            UpdateUI();
        }
        #endregion
    }
}
