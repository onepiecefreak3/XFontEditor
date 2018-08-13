using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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
                font.image_0.Save(Path.Combine(fd.SelectedPath, "font_image0.png"));
                font.image_1.Save(Path.Combine(fd.SelectedPath, "font_image1.png"));
                font.image_2.Save(Path.Combine(fd.SelectedPath, "font_image2.png"));
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new FolderBrowserDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                //Import images
                if (File.Exists(Path.Combine(fd.SelectedPath, "font_image0.png")))
                    font.image_0 = new Bitmap(Path.Combine(fd.SelectedPath, "font_image0.png"));
                if (File.Exists(Path.Combine(fd.SelectedPath, "font_image1.png")))
                    font.image_1 = new Bitmap(Path.Combine(fd.SelectedPath, "font_image1.png"));
                if (File.Exists(Path.Combine(fd.SelectedPath, "font_image2.png")))
                    font.image_2 = new Bitmap(Path.Combine(fd.SelectedPath, "font_image2.png"));

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
            foreach (var character in font.dicGlyphLarge)
            {
                largeDict.Rows.Add(
                    character.Key,
                    character.Value.ColorChannel,
                    character.Value.ImageOffsetX,
                    character.Value.ImageOffsetY,
                    font.lstCharSizeInfoLarge[character.Value.code_point].char_width,
                    font.lstCharSizeInfoLarge[character.Value.code_point].char_height,
                    character.Value.CharWidth);
                largeDict.Rows[id].HeaderCell.Value = String.Format("{0}", id++);
            }

            id = 0;
            foreach (var character in font.dicGlyphSmall)
            {
                smallDict.Rows.Add(
                    character.Key,
                    character.Value.ColorChannel,
                    character.Value.ImageOffsetX,
                    character.Value.ImageOffsetY,
                    font.lstCharSizeInfoSmall[character.Value.code_point].char_width,
                    font.lstCharSizeInfoSmall[character.Value.code_point].char_height,
                    character.Value.CharWidth);
                smallDict.Rows[id].HeaderCell.Value = String.Format("{0}", id++);
            }

            largeDict.CellValueChanged += largeDict_CellValueChanged;
            smallDict.CellValueChanged += smallDict_CellValueChanged;
        }

        void LoadImages()
        {
            image0.Image = new Bitmap(font.image_0.Width, font.image_0.Height);
            image1.Image = new Bitmap(font.image_1.Width, font.image_1.Height);
            image2.Image = new Bitmap(font.image_2.Width, font.image_2.Height);

            Graphics g;

            g = Graphics.FromImage(image0.Image);
            //g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, image0.Image.Width, image0.Image.Height));
            g.DrawImage(font.image_0, new PointF(0, 0));

            g = Graphics.FromImage(image1.Image);
            //g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, image1.Image.Width, image1.Image.Height));
            g.DrawImage(font.image_1, new PointF(0, 0));

            g = Graphics.FromImage(image2.Image);
            //g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, image2.Image.Width, image2.Image.Height));
            g.DrawImage(font.image_2, new PointF(0, 0));
        }

        void DrawCharInfo()
        {
            foreach (var character in font.dicGlyphLarge)
            {
                Graphics g = null;
                switch (character.Value.ColorChannel)
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
                g.DrawRectangle(
                     new Pen(c),
                     new Rectangle(
                         character.Value.ImageOffsetX,
                         character.Value.ImageOffsetY,
                         font.lstCharSizeInfoLarge[character.Value.code_point].char_width,
                         font.lstCharSizeInfoLarge[character.Value.code_point].char_height)
                         );
            }

            foreach (var character in font.dicGlyphSmall)
            {
                Graphics g = null;
                switch (character.Value.ColorChannel)
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
                g.DrawRectangle(
                    new Pen(c),
                    new Rectangle(
                        character.Value.ImageOffsetX,
                        character.Value.ImageOffsetY,
                        font.lstCharSizeInfoSmall[character.Value.code_point].char_width,
                        font.lstCharSizeInfoSmall[character.Value.code_point].char_height)
                        );
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
            var origGlyph = font.dicGlyphLarge[Convert.ToChar(glyphRow.Cells["Character"].Value)];

            font.dicGlyphLarge[Convert.ToChar(glyphRow.Cells["Character"].Value)] =
                new Format.XF.CharacterMap
                {
                    code_point = Convert.ToChar(glyphRow.Cells["Character"].Value),
                    char_size = (ushort)((origGlyph.CharSizeInfoIndex & 0x3FF) | ((Convert.ToUInt16(glyphRow.Cells["GlyphWidth"].Value) & 0x3F) << 10)),
                    image_offset = Convert.ToInt32(glyphRow.Cells["ImageID"].Value) | (Convert.ToInt32(glyphRow.Cells["X"].Value) << 4) | (Convert.ToInt32(glyphRow.Cells["Y"].Value) << 18)
                };

            font.lstCharSizeInfoLarge[origGlyph.code_point].char_height = Convert.ToByte(glyphRow.Cells["Height"].Value);
            font.lstCharSizeInfoLarge[origGlyph.code_point].char_width = Convert.ToByte(glyphRow.Cells["Width"].Value);

            LoadImages();
            DrawCharInfo();
        }

        private void smallDict_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var glyphRow = smallDict.Rows[e.RowIndex];
            var origGlyph = font.dicGlyphSmall[Convert.ToChar(glyphRow.Cells["Character2"].Value)];

            font.dicGlyphLarge[Convert.ToChar(glyphRow.Cells["Character2"].Value)] =
                new Format.XF.CharacterMap
                {
                    code_point = Convert.ToChar(glyphRow.Cells["Character2"].Value),
                    char_size = (ushort)((origGlyph.CharSizeInfoIndex & 0x3FF) | ((Convert.ToUInt16(glyphRow.Cells["GlyphWidth2"].Value) & 0x3F) << 10)),
                    image_offset = Convert.ToInt32(glyphRow.Cells["ImageID2"].Value) | (Convert.ToInt32(glyphRow.Cells["X2"].Value) << 4) | (Convert.ToInt32(glyphRow.Cells["Y2"].Value) << 18)
                };

            font.lstCharSizeInfoSmall[origGlyph.code_point].char_height = Convert.ToByte(glyphRow.Cells["Height2"].Value);
            font.lstCharSizeInfoSmall[origGlyph.code_point].char_width = Convert.ToByte(glyphRow.Cells["Width2"].Value);

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
