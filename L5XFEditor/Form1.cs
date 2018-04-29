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
    public partial class Form1 : Form
    {
        Format.XF font = null;

        Bitmap red = null;
        Bitmap green = null;
        Bitmap blue = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("./font.xf"))
            {
                font = new Format.XF(File.OpenRead("./font.xf"));

                dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
                dataGridView2.CellValueChanged -= dataGridView1_CellValueChanged;

                LoadDataGrid();
                LoadImages();
                DrawCharInfo();

                dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
                dataGridView2.CellValueChanged += dataGridView1_CellValueChanged;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                font = new Format.XF(File.OpenRead(fd.FileName));

                dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;
                dataGridView2.CellValueChanged -= dataGridView1_CellValueChanged;

                LoadDataGrid();
                LoadImages();
                DrawCharInfo();

                dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
                dataGridView2.CellValueChanged += dataGridView1_CellValueChanged;
            }

            (menuStrip1.Items[0] as ToolStripMenuItem).DropDownItems[1].Enabled = true;
            (menuStrip1.Items[1] as ToolStripMenuItem).DropDownItems[0].Enabled = true;
            (menuStrip1.Items[1] as ToolStripMenuItem).DropDownItems[1].Enabled = true;
        }

        void LoadDataGrid()
        {
            //for normal glyphs
            dataGridView1.Rows.Clear();
            //for small glyphs
            dataGridView2.Rows.Clear();

            int id = 0;
            foreach (var character in font.dicGlyphLarge)
            {
                dataGridView1.Rows.Add(character.Key, character.Value.ColorChannel,
                    character.Value.ImageOffsetX, character.Value.ImageOffsetY,
                    font.lstCharSizeInfoLarge[character.Value.code_point].glyph_width, font.lstCharSizeInfoLarge[character.Value.code_point].glyph_height);
                dataGridView1.Rows[id].HeaderCell.Value = String.Format("{0}", id++);
            }

            id = 0;
            foreach (var character in font.dicGlyphSmall)
            {
                dataGridView2.Rows.Add(character.Key, character.Value.ColorChannel,
                    character.Value.ImageOffsetX, character.Value.ImageOffsetY,
                    font.lstCharSizeInfoSmall[character.Value.code_point].glyph_width, font.lstCharSizeInfoSmall[character.Value.code_point].glyph_height);
                dataGridView2.Rows[id].HeaderCell.Value = String.Format("{0}", id++);
            }
        }

        void LoadImages()
        {
            pictureBox1.Image = new Bitmap(font.image_0.Width, font.image_0.Height);
            pictureBox2.Image = new Bitmap(font.image_1.Width, font.image_1.Height);
            pictureBox3.Image = new Bitmap(font.image_2.Width, font.image_2.Height);

            Graphics g;
            g = Graphics.FromImage(pictureBox1.Image);
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height));
            g.DrawImage(font.image_0, new PointF(0, 0));
            g = Graphics.FromImage(pictureBox2.Image);
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, pictureBox2.Image.Width, pictureBox2.Image.Height));
            g.DrawImage(font.image_1, new PointF(0, 0));
            g = Graphics.FromImage(pictureBox3.Image);
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, pictureBox3.Image.Width, pictureBox3.Image.Height));
            g.DrawImage(font.image_2, new PointF(0, 0));

            //Resize PictureBoxes to fit image
            pictureBox1.Width = font.image_0.Width;
            pictureBox1.Height = font.image_0.Height;

            pictureBox2.Width = font.image_1.Width;
            pictureBox2.Height = font.image_1.Height;

            pictureBox3.Width = font.image_2.Width;
            pictureBox3.Height = font.image_2.Height;
        }

        void DrawCharInfo()
        {
            foreach (var character in font.dicGlyphLarge)
            {
                Graphics g = null;
                switch (character.Value.ColorChannel)
                {
                    case 0:
                        g = Graphics.FromImage(pictureBox1.Image);
                        break;
                    case 1:
                        g = Graphics.FromImage(pictureBox2.Image);
                        break;
                    case 2:
                        g = Graphics.FromImage(pictureBox3.Image);
                        break;
                }

                g.DrawRectangle(
                    new Pen(Color.DarkRed),
                    new Rectangle(
                        character.Value.ImageOffsetX,
                        character.Value.ImageOffsetY,
                        font.lstCharSizeInfoLarge[character.Value.code_point].glyph_width,
                        font.lstCharSizeInfoLarge[character.Value.code_point].glyph_height)
                        );
            }
            foreach (var character in font.dicGlyphSmall)
            {
                Graphics g = null;
                switch (character.Value.ColorChannel)
                {
                    case 0:
                        g = Graphics.FromImage(pictureBox1.Image);
                        break;
                    case 1:
                        g = Graphics.FromImage(pictureBox2.Image);
                        break;
                    case 2:
                        g = Graphics.FromImage(pictureBox3.Image);
                        break;
                }

                g.DrawRectangle(
                    new Pen(Color.DarkRed),
                    new Rectangle(
                        character.Value.ImageOffsetX,
                        character.Value.ImageOffsetY,
                        font.lstCharSizeInfoSmall[character.Value.code_point].glyph_width,
                        font.lstCharSizeInfoSmall[character.Value.code_point].glyph_height)
                        );
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sd = new SaveFileDialog();

            if (sd.ShowDialog() == DialogResult.OK)
            {
                font.Save(File.Create(sd.FileName));

                MessageBox.Show($"Font was successfully saved to {sd.FileName}.", "Font saved.", MessageBoxButtons.OK);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Image 0")
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;
            }
            else if (comboBox1.Text == "Image 1")
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
                pictureBox3.Visible = false;
            }
            else if (comboBox1.Text == "Image 2")
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = false;
                pictureBox3.Visible = true;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var glyphRow = dataGridView1.Rows[e.RowIndex];
            var origGlyph = font.dicGlyphLarge[(char)dataGridView1.Rows[e.RowIndex].Cells["Character"].Value];
            font.dicGlyphLarge[(char)dataGridView1.Rows[e.RowIndex].Cells["Character"].Value] =
                new Format.XF.CharacterMap
                {
                    code_point = (char)glyphRow.Cells["Character"].Value,
                    char_size = origGlyph.char_size,
                    image_offset = (int)glyphRow.Cells["Image"].Value | ((int)glyphRow.Cells["X"].Value << 4) | ((int)glyphRow.Cells["Y"].Value << 18)
                };

            font.lstCharSizeInfoLarge[origGlyph.code_point].glyph_height = Convert.ToByte((string)glyphRow.Cells["Height"].Value);
            font.lstCharSizeInfoLarge[origGlyph.code_point].glyph_width = (byte)glyphRow.Cells["Width"].Value;

            LoadImages();
            DrawCharInfo();
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var glyphRow = dataGridView2.Rows[e.RowIndex];
            var origGlyph = font.dicGlyphSmall[(char)dataGridView2.Rows[e.RowIndex].Cells["Character"].Value];
            font.dicGlyphLarge[(char)dataGridView2.Rows[e.RowIndex].Cells["Character"].Value] =
                new Format.XF.CharacterMap
                {
                    code_point = (char)glyphRow.Cells["Character"].Value,
                    char_size = origGlyph.char_size,
                    image_offset = (int)glyphRow.Cells["Image"].Value | ((int)glyphRow.Cells["X"].Value << 4) | ((int)glyphRow.Cells["Y"].Value << 18)
                };

            font.lstCharSizeInfoSmall[origGlyph.code_point].glyph_height = Convert.ToByte((string)glyphRow.Cells["Height"].Value);
            font.lstCharSizeInfoSmall[origGlyph.code_point].glyph_width = (byte)glyphRow.Cells["Width"].Value;

            LoadImages();
            DrawCharInfo();
        }
    }
}
