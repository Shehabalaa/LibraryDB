﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Location of Panels like Book Sections ... is 225,35
// Label to show num of available seats AvSeatsLabel
// El7aga.Font = new Font("",size,FontStyle.bold);
/*Each panel has a location (225,60) size 214*260 has a pic box of size 214*192 has a label (1,200)*/
namespace Egypt_National_Library
{

    public partial class User_Form : Form
    {
        Panel ActivePanel = new Panel();
        string CurrentPanel = "";
        int User_ID;
        DataTable dataTable = new DataTable();
        Controller Controller_OBJ;
        PictureBox[] Bookpictureboxes, Storypictureboxes, Computerpictureboxes, Instrumentspictureboxes,All_Itemspictureboxes;
        Panel[] BookPanels = null, StoryPanels = null, ComputerPanels = null, InstrumentPanels = null, All_ItemsPanels = null;
        int NumofBookpictureboxes = 0, NumofStorypictureboxes = 0, NumofInstrumentspictureboxes = 0, NumofComputerpictureboxes = 0, NumofSoftwarepictureboxes = 0, Numof_AllItemspictureboxes = 0;
        public User_Form(string UserName,string Rate,byte[] image,int ID)
        {
            InitializeComponent();
            Controller_OBJ = new Controller();
            MemoryStream ms = new MemoryStream(image);
            UserPicturebox.Image = Image.FromStream(ms);
            UserPicturebox.Refresh();
            NameLabel.Text = UserName;
            RateLabel.Text = Rate;
            User_ID = ID;

        }
        private void User_Form_Load(object sender, EventArgs e)
        {
            Point Location = new Point(225, 35);
            LibDepsPanel.Location = MusicDepPanel.Location = BookDepPanel.Location = StoryDepPanel.Location = CmpDepPanel.Location = FilterPanel.Location = Location;
            dataTable = Controller_OBJ.GetBookDepAvailableSeats();
            AvSeatsLabel.Text = dataTable.Rows[0].ItemArray[0].ToString();
            dataTable = Controller_OBJ.GetStoryDepAvailableSeats();
            StoryDepAvailableSeats.Text= dataTable.Rows[0].ItemArray[0].ToString();
            dataTable = Controller_OBJ.GetMusicDepAvailableSeats();
            MusicDepAvSeats.Text = dataTable.Rows[0].ItemArray[0].ToString();
            dataTable = Controller_OBJ.GetCmpDepAvailableSeats();
            CmpDepAvSeats.Text= dataTable.Rows[0].ItemArray[0].ToString();

        }
       
        private void closeButton_Click(object sender, EventArgs e) { this.Close(); Application.Exit(); }
        private void LibDepBtn_Click(object sender, EventArgs e)    { ActivatePanel(LibDepsPanel);   }
        private void BookDepBtn_Click(object sender, EventArgs e)  { UpdateAvailableSeats(); ActivatePanel(BookDepPanel);   }
        private void StoryDepBtn_Click(object sender, EventArgs e) { UpdateAvailableSeats(); ActivatePanel(StoryDepPanel);  }
        private void MusicDepBtn_Click(object sender, EventArgs e) { UpdateAvailableSeats(); ActivatePanel(MusicDepPanel); }
        private void CmpDepBtn_Click(object sender, EventArgs e) { UpdateAvailableSeats(); ActivatePanel(CmpDepPanel); }
        private void FilterBtn_Click(object sender, EventArgs e) { ActivatePanel(FilterPanel); }
        private void ActivatePanel(Panel panel){ if (!panel.Visible) { ActivePanel.Visible = false; ActivePanel = panel; ActivePanel.Show(); } else ActivePanel.Visible = false; }
        private void Label_Click(object sender,EventArgs e) { }
        private void Book_PictureBox_Click(object sender, EventArgs e)
        {
           Book_Details_Form Form;
            string ID = "", Section = "";
            int BookUserID=0;
            for (int i=0;i<NumofBookpictureboxes;++i)
                if(sender.Equals(Bookpictureboxes[i]))
                {
                    string Temp = Bookpictureboxes[i].Name.ToString();
                    for (int k = 0; k < Temp.Length; ++k) if (Temp[k] == '/') { ID = Temp; ID = ID.Remove(0, k + 1); break; }
                    for(int k = 0; k < ID.Length;++k) if (ID[k] == '/') { Section = ID; Section = Section.Remove(0, k + 1); ID = ID.Remove(k, Section.Length + 1); break; }
                    DataTable dt = Controller_OBJ.GetBookByName(int.Parse(ID), 1, Section);

                    
                    if (!DBNull.Value.Equals(dt.Rows[0].ItemArray[11]))
                    {
                        BookUserID = int.Parse(dt.Rows[0].ItemArray[11].ToString());
                        //not null
                    }
                    else
                    {
                            BookUserID = 0;
                            //null
                    }

                    Form = new Book_Details_Form(dt.Rows[0].ItemArray[6].ToString(),
                        dt.Rows[0].ItemArray[0].ToString(),
                        dt.Rows[0].ItemArray[1].ToString(),
                        dt.Rows[0].ItemArray[2].ToString(),
                        dt.Rows[0].ItemArray[7].ToString(),
                        dt.Rows[0].ItemArray[8].ToString(),
                        dt.Rows[0].ItemArray[12].ToString(),
                        dt.Rows[0].ItemArray[5].ToString(),
                        (byte[])dt.Rows[0].ItemArray[14],
                        User_ID, dt.Rows[0].ItemArray[9].ToString(),
                        BookUserID,
                        int.Parse(dt.Rows[0].ItemArray[3].ToString()),
                        int.Parse(dt.Rows[0].ItemArray[4].ToString()));
                    Form.Show();
                    return;
                }
        }
        private void UpdateAvailableSeats()
        {
            dataTable = Controller_OBJ.GetBookDepAvailableSeats();
            AvSeatsLabel.Text = dataTable.Rows[0].ItemArray[0].ToString();
            dataTable = Controller_OBJ.GetStoryDepAvailableSeats();
            StoryDepAvailableSeats.Text = dataTable.Rows[0].ItemArray[0].ToString();
            dataTable = Controller_OBJ.GetMusicDepAvailableSeats();
            MusicDepAvSeats.Text = dataTable.Rows[0].ItemArray[0].ToString();
            dataTable = Controller_OBJ.GetCmpDepAvailableSeats();
            CmpDepAvSeats.Text = dataTable.Rows[0].ItemArray[0].ToString();
        }
        private void DeletePanels()
        {
            if (BookPanels != null) 
                for (int i = 0; i < BookPanels.Length; ++i)
                    for (int j = 0; j < Controls.Count; ++j)
                        if (BookPanels[i] == Controls[j]) { Controls.Remove(BookPanels[i]); break; }

            if (StoryPanels != null)
                for (int i = 0; i < StoryPanels.Length; ++i)
                    for (int j = 0; j < Controls.Count; ++j)
                        if (StoryPanels[i] == Controls[j]) { Controls.Remove(StoryPanels[i]); break; }

            if (InstrumentPanels != null) 
                for (int i = 0; i < InstrumentPanels.Length; ++i)
                    for (int j = 0; j < Controls.Count; ++j)
                        if (InstrumentPanels[i] == Controls[j]) { Controls.Remove(InstrumentPanels[i]); break; }

            if (ComputerPanels != null) 
                for (int i = 0; i < ComputerPanels.Length; ++i)
                    for (int j = 0; j < Controls.Count; ++j)
                        if (ComputerPanels[i] == Controls[j]) { Controls.Remove(ComputerPanels[i]); break; }

            BookPanels = StoryPanels = ComputerPanels = InstrumentPanels = null;
        }
        private void Story_PictureBox_Click(object sender, EventArgs e)
        {
            Story_Details_Form Form;
            int StoryUserID = 0;
            for (int i = 0; i < NumofStorypictureboxes; ++i)
                if (sender.Equals(Storypictureboxes[i]))
                {
                    string Temp = Storypictureboxes[i].Name.ToString();
                    string ID = "", Section = "";
                    for (int k = 0; k < Temp.Length; ++k) if (Temp[k] == '/') { ID = Temp; ID = ID.Remove(0, k + 1); break; }
                    for (int k = 0; k < ID.Length; ++k) if (ID[k] == '/') { Section = ID; Section = Section.Remove(0, k + 1); ID = ID.Remove(k, Section.Length + 1); break; }
                    DataTable dt = Controller_OBJ.GetStoryByName(int.Parse(ID), 2, Section);

                    if (!DBNull.Value.Equals(dt.Rows[0].ItemArray[6]))
                    {
                        StoryUserID = int.Parse(dt.Rows[0].ItemArray[6].ToString());
                        //not null
                    }
                    else
                    {
                        StoryUserID = 0;
                        //null
                    }

                    Form = new Story_Details_Form(dt.Rows[0].ItemArray[1].ToString(),dt.Rows[0].ItemArray[2].ToString(),
                        dt.Rows[0].ItemArray[4].ToString(), (byte[])dt.Rows[0].ItemArray[7], User_ID, dt.Rows[0].ItemArray[9].ToString(),
                        StoryUserID,
                        int.Parse(dt.Rows[0].ItemArray[0].ToString()),
                         int.Parse(dt.Rows[0].ItemArray[8].ToString()));
                    Form.Show();
                    return;
                }
        }
        private void Instrument_PictureBox_Click(object sender, EventArgs e) 
        {
           Music_Instrument_Details_Form Form;
            string ID = "", Section = "";
            int InstrumentUserID = 0;
            for (int i = 0; i < NumofInstrumentspictureboxes; ++i)
                if (sender.Equals(Instrumentspictureboxes[i]))
                {
                    string Temp = Instrumentspictureboxes[i].Name.ToString();
                    for (int k = 0; k < Temp.Length; ++k) if (Temp[k] == '/') { ID = Temp; ID = ID.Remove(0, k + 1); break; }
                    for (int k = 0; k < ID.Length; ++k) if (ID[k] == '/') { Section = ID; Section = Section.Remove(0, k + 1); ID = ID.Remove(k, Section.Length + 1); break; }
                    DataTable dt = Controller_OBJ.GetMusical_InstrumentByName(int.Parse(ID), 3, Section);
                    if (!DBNull.Value.Equals(dt.Rows[0].ItemArray[4]))
                    {
                       InstrumentUserID = int.Parse(dt.Rows[0].ItemArray[4].ToString());
                        //not null
                    }
                    else
                    {
                        InstrumentUserID = 0;
                        //null
                    }

                    Form = new Music_Instrument_Details_Form(dt.Rows[0].ItemArray[1].ToString(), dt.Rows[0].ItemArray[3].ToString(), dt.Rows[0].ItemArray[5].ToString(), (byte[])dt.Rows[0].ItemArray[7], User_ID,
                        dt.Rows[0].ItemArray[2].ToString(),
                        InstrumentUserID,
                        int.Parse(dt.Rows[0].ItemArray[0].ToString()),
                        int.Parse(dt.Rows[0].ItemArray[8].ToString()));

                    Form.Show();
                    return;
                }
        }
        private void Computer_PictureBox_Click(object sender, EventArgs e)
        {
            Computer_Details_Form Form; string ID = "", Section = "";
            int CmpUserID = 0;
            for (int i = 0; i < NumofComputerpictureboxes; ++i)
                if (sender.Equals(Computerpictureboxes[i]))
                {
                    string Temp = Computerpictureboxes[i].Name.ToString();
                    for (int k = 0; k < Temp.Length; ++k) if (Temp[k] == '/') { ID = Temp; ID = ID.Remove(0, k + 1); break; }
                    for (int k = 0; k < ID.Length; ++k) if (ID[k] == '/') { Section = ID; Section = Section.Remove(0, k + 1); ID = ID.Remove(k, Section.Length + 1); break; }
                    DataTable dt = Controller_OBJ.GetComputerSoftwares(int.Parse(ID), 4, Section);
                    DataTable dt1 = Controller_OBJ.GetComputerByName(int.Parse(ID), 4, Section);
                    if (!DBNull.Value.Equals(dt1.Rows[0].ItemArray[4]))
                    {
                        CmpUserID = int.Parse(dt1.Rows[0].ItemArray[4].ToString());
                        //not null
                    }
                    else
                    {
                        CmpUserID = 0;
                        //null
                    }

                    Panel[] Panels = GetSoftwares_Panels(ID, Section);
                    Form = new Computer_Details_Form(Panels, Panels.Length, dt1.Rows[0].ItemArray[3].ToString(), dt1.Rows[0].ItemArray[5].ToString(), (byte[])dt1.Rows[0].ItemArray[7], dt1.Rows[0].ItemArray[1].ToString(), User_ID
                        ,dt1.Rows[0].ItemArray[2].ToString(),
                        CmpUserID,
                        int.Parse(dt1.Rows[0].ItemArray[0].ToString()),
                         int.Parse(dt1.Rows[0].ItemArray[8].ToString()));
                    Form.Show();
                    return;
                }
        }

        // Show User Reservations //
        private void MyBooksBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "MyBook"; Show_MyBooks_Panels(); }
        private void MyStoriesBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "MyStory"; Show_MyStories_Panels(); }
        private void MyInstrumentsBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "MyMusicInstrument"; Show_MyInstruments_Panels(); }
        private void AddComputersBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "MyComputer"; Show_MyComputers_Panels(); }
        //-----------------------------------------------------------------------------------------------------------//
        //Show All Library Store//
        private void DiscoverBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Library"; ShowAllItems(); }
        //----------------------------------------------------------------------------------------------------------//
        //Book Department Buttons Click ,, showing panels of each Book Department//
        private void ProgrammingDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book/Programming"; Show_Book_Panels("Programming", 1); }
        private void BabyCareDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book/Baby Care"; Show_Book_Panels("Baby Care", 1); }
        private void ScienceDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book/Science"; Show_Book_Panels("Science", 1); }
        private void ReligiousDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book/Religious"; Show_Book_Panels("Religious", 1); }
        private void GeographyDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book/Geographic"; Show_Book_Panels("Geographic", 1); }
        private void HealthDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book/Health"; Show_Book_Panels("Health", 1); }
        private void HumanDevDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book/Human Development"; Show_Book_Panels("Human Development", 1); }
        private void NutritionDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book/Nutrition"; Show_Book_Panels("Nutrition", 1); }
        private void ArabLitDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book/Literature"; Show_Book_Panels("Literature", 1); }
        private void HistoryDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book/History"; Show_Book_Panels("History", 1); }
        private void MathsDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book/Maths"; Show_Book_Panels("Maths", 1); }
        //----------------------------------------------------------------------------------------------------------//
        //Story Department Buttons Click ,, showing panels of each Story Department//
        private void HorrorDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Story/Horror"; Show_Stories_Panels("Horror", 2); }
        private void RomanticDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Story/Romantic"; Show_Stories_Panels("Romantic", 2); }
        private void DramaDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Story/Drama"; Show_Stories_Panels("Drama", 2); }
        private void LearningDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Story/Learning"; Show_Stories_Panels("Learning", 2); }
        //----------------------------------------------------------------------------------------------------------//
        //Music Department Buttons Click ,, showing panels of each Music Department//
        private void DrumsDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Music Instrument/Drums"; Show_Instruments_Panels("Drums", 3); }
        private void GuitarDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Music Instrument/Guitar"; Show_Instruments_Panels("Guitar", 3); }
        private void MicrophonesDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Music Instrument/Microphones&HeadSets"; Show_Instruments_Panels("Microphones&HeadSets", 3); }
        private void PianoDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Music Instrument/Piano"; Show_Instruments_Panels("Piano", 3); }
        //----------------------------------------------------------------------------------------------------------//
        //Computer Department Buttons Click ,, showing panels of each Computer Department//
        private void ArchitectureDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Computer/Architecture"; Show_Computers_Panels("Architecture", 4); }
        private void PowerEngineeringDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Computer/Power Engineering"; Show_Computers_Panels("Power Engineering", 4); }
        private void SoftwareDepBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Computer/SoftWare Engineering"; Show_Computers_Panels("SoftWare Engineering", 4); }
        //----------------------------------------------------------------------------------------------------------//
        //All Of Books,Instrumsents,Computers,Stories //
        private void ShowAll_BooksBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Book"; Show_AllBooks_Panels(); }
        private void ShowAll_StoriesBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Story"; Show_AllStories_Panels(); }
        private void ShowAll_IstrumentBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Music Instrument"; Show_All_Instrumetns_Panels(); }
        private void ShowAll_CmpBtn_Click(object sender, EventArgs e) { DeletePanels(); CurrentPanel = "Computer"; Show_AllComputers_Panels(); }
        //----------------------------------------------------------------------------------------------------------//
        //Filtered Things//
        private void TopPricebtn_Click(object sender, EventArgs e)
        {
            DeletePanels();
            string Section = "";
            string Temp = CurrentPanel;
            for (int k = 0; k < Temp.Length; ++k) if (Temp[k] == '/') { Section = Temp; Section = Section.Remove(0, k + 1); break; }
            Show_Filtered_Panels("TopPrice", Section);
        }
        private void TopRatedbtn_Click(object sender, EventArgs e)
        {
            DeletePanels();
            string Section = "";
            string Temp = CurrentPanel;
            for (int k = 0; k < Temp.Length; ++k) if (Temp[k] == '/') { Section = Temp; Section = Section.Remove(0, k + 1); break; }
            Show_Filtered_Panels("TopRated", "");
        }
        private void LowPricebtn_Click(object sender, EventArgs e)
        {
            DeletePanels();
            string Section = "";
            string Temp = CurrentPanel;
            for (int k = 0; k < Temp.Length; ++k) if (Temp[k] == '/') { Section = Temp; Section = Section.Remove(0, k + 1); break; }
            Show_Filtered_Panels("LowPrice", "");
        }
        private void LowRatedbtn_Click(object sender, EventArgs e)
        {
            DeletePanels();
            string Section = "";
            string Temp = CurrentPanel;
            for (int k = 0; k < Temp.Length; ++k) if (Temp[k] == '/') { Section = Temp; Section = Section.Remove(0, k + 1); break; }
            Show_Filtered_Panels("LowRated", "");
        }
        private void AvailableBtn_Click(object sender, EventArgs e)
        {
            DeletePanels();
            string Section = "";
            string Temp = CurrentPanel;
            for (int k = 0; k < Temp.Length; ++k) if (Temp[k] == '/') { Section = Temp; Section = Section.Remove(0, k + 1); break; }
            Show_Filtered_Panels("Available", "");
        }
        private void InUseBtn_Click(object sender, EventArgs e)
        {
            DeletePanels();
            string Section = "";
            string Temp = CurrentPanel;
            for (int k = 0; k < Temp.Length; ++k) if (Temp[k] == '/') { Section = Temp; Section = Section.Remove(0, k + 1); break; }
            Show_Filtered_Panels("NotAvailable", "");
        }
        //*-----------------------------------------//
        private Panel CreatePanel(int i,Color Backcolor,int width,int height,int locationX,int locationY)
        {
            Panel panel = new Panel();
            panel.Name = "myPanel" + i.ToString();
            panel.BackColor = Backcolor;
            panel.Width = width;
            panel.Height = height;
            panel.Location = new Point(locationX, locationY);
            return panel;
        }
        private void InesertControlToPanel(ref Panel panel, ref LinkLabel label, ref PictureBox pictureBox)
        {
            panel.Controls.Add(pictureBox);
            panel.Controls.Add(label);
            //-----------Edit Options for the picture box-----------------//
            panel.Controls[0].Width = panel.Width;
            panel.Controls[0].Height = 192;
            panel.Controls[0].Location = new Point(0, 0);

            //----------Edit Options for the Label------------------//
            panel.Controls[1].Width = panel.Width;
            panel.Controls[1].Height = 60;
            panel.Controls[1].Location = new Point(1, 200);

        }

        

        private void AboutLibraryBtn_Click(object sender, EventArgs e)
        {

        }

        private LinkLabel CreateLabel(int i,string text)
        {
            LinkLabel label = new LinkLabel();
            label.Name = "myLabel" + i.ToString();
            label.Font = new Font("Century Gothic", 10, FontStyle.Bold);
            label.LinkBehavior = LinkBehavior.NeverUnderline;
            label.TextAlign = ContentAlignment.TopCenter;
            label.LinkColor = Color.FromArgb(1, 66, 66, 66);
            label.ActiveLinkColor = Color.DarkSalmon;
            label.Text = text;
            return label;
        }
        private PictureBox CreatePicturebox(int i,byte[] image)
        {
            PictureBox picturebox = new PictureBox();
            picturebox.Name = "myPictureBox" + i.ToString();
            MemoryStream ms = new MemoryStream(image);
            picturebox.Image = Image.FromStream(ms);
            picturebox.SizeMode = PictureBoxSizeMode.Zoom;
            picturebox.Refresh();
            return picturebox;
        }
        private Label CreateNormalLabel(int i, string text)
        {
            Label label = new Label();
            label.Name = "myLabel" + i.ToString();
            label.Font = new Font("Century Gothic", 12, FontStyle.Bold);
            label.TextAlign = ContentAlignment.TopCenter;
            label.Text = text;
            return label;
        }


        private void Show_Book_Panels(string type,int libDepID)
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetSpecificBooks(type, libDepID); if (dataTable == null) return;
            int Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count;
            Panel[] Panels = BookPanels = new Panel[Num_OF_Panels];
            Bookpictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[6].ToString());
                Bookpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[14]);
                Bookpictureboxes[i].Name = Labels[i].Text + "/"+ dataTable.Rows[i].ItemArray[3].ToString()+"/"+ dataTable.Rows[i].ItemArray[12].ToString();
                Bookpictureboxes[i].Click += new EventHandler(this.Book_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Bookpictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private void Show_Stories_Panels(string type, int libDepID)
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetSpecificStories(type, libDepID); if (dataTable == null) return;
            int Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count;
            Panel[] Panels = StoryPanels = new Panel[Num_OF_Panels];
            Storypictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[1].ToString());
                Storypictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Storypictureboxes[i].Name = "Stories" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[4].ToString();
                Storypictureboxes[i].Click += new System.EventHandler(this.Story_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Storypictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }

        private void Show_Instruments_Panels(string type, int libDepID)
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetSpecificInstruments(type, libDepID); if (dataTable == null) return;
            int Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count;
            Panel[] Panels = InstrumentPanels = new Panel[Num_OF_Panels];
            Instrumentspictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[1].ToString());
                Instrumentspictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Instrumentspictureboxes[i].Name = "Instruments" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                Instrumentspictureboxes[i].Click += new System.EventHandler(this.Instrument_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Instrumentspictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private void Show_Computers_Panels(string type, int libDepID)
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetSpecificComputers(type, libDepID); if (dataTable == null) return;
            int Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count;
            Panel[] Panels = ComputerPanels = new Panel[Num_OF_Panels];
            Computerpictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[5].ToString());
                Computerpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Computerpictureboxes[i].Name = "Computers" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                Computerpictureboxes[i].Click += new System.EventHandler(this.Computer_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Computerpictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private Panel[] GetSoftwares_Panels(string ID,string Section)
        {
            ActivePanel.Visible = false;
            DataTable dt = Controller_OBJ.GetComputerSoftwares(int.Parse(ID), 4, Section); 
            int Num_OF_Panels = NumofSoftwarepictureboxes = dt.Rows.Count;
            Panel[] Panels = new Panel[Num_OF_Panels];
            PictureBox[]Softwarepictureboxes = new PictureBox[Num_OF_Panels];
            Label[] Labels = new Label[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 160, 310, 50  +180 *(incrementer++) , 250);
                Labels[i] = CreateNormalLabel(i, dt.Rows[i].ItemArray[0].ToString());
                Softwarepictureboxes[i] = CreatePicturebox(i, (byte[])dt.Rows[i].ItemArray[4]);
                Softwarepictureboxes[i].Name = "Software" + "/" + dt.Rows[i].ItemArray[0].ToString() + "/" + dt.Rows[i].ItemArray[1].ToString() + "/" + dt.Rows[i].ItemArray[2].ToString();
                Softwarepictureboxes[i].SizeMode = PictureBoxSizeMode.StretchImage;
                Panels[i].Controls.Add(Softwarepictureboxes[i]);
                Panels[i].Controls.Add(Labels[i]);
                //-----------Edit Options for the picture box-----------------//
                Panels[i].Controls[0].Width = Panels[i].Width;
                Panels[i].Controls[0].Height = 155;
                Panels[i].Controls[0].Location = new Point(0, 0);
                //----------Edit Options for the Label------------------//
                Panels[i].Controls[1].Width = Panels[i].Width;
                Panels[i].Controls[1].Height = 60;
                Panels[i].Controls[1].Location = new Point(2, 170);
                if (Panels[i].Controls[1].Text == " Calculatoredge")
                    Panels[i].Controls[1].Text = "Calculatoredge";
            }
            return Panels;
        }
        private void Show_AllBooks_Panels()
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetAllBooks(); if (dataTable == null) return;
            int Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count;
            Panel[] Panels = BookPanels = new Panel[Num_OF_Panels];
            Bookpictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Book/" + dataTable.Rows[i].ItemArray[12].ToString() + "/" + dataTable.Rows[i].ItemArray[6].ToString());
                Bookpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[14]);
                Bookpictureboxes[i].Name = "Book" + "/" + dataTable.Rows[i].ItemArray[3].ToString() + "/" + dataTable.Rows[i].ItemArray[12].ToString();
                Bookpictureboxes[i].Click += new EventHandler(this.Book_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Bookpictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private void Show_All_Instrumetns_Panels()
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetAllMusicInstruments(); if (dataTable == null) return;
            int Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count;
            Panel[] Panels = InstrumentPanels = new Panel[Num_OF_Panels];
            Instrumentspictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Music Instrument/" + dataTable.Rows[i].ItemArray[5].ToString() + "/" + dataTable.Rows[i].ItemArray[1].ToString());
                Instrumentspictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Instrumentspictureboxes[i].Name = "Music Instrument" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                Instrumentspictureboxes[i].Click += new EventHandler(this.Instrument_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Instrumentspictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private void Show_AllComputers_Panels()
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetAllComputers(); if (dataTable == null) return;
            int Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count;
            Panel[] Panels = ComputerPanels = new Panel[Num_OF_Panels];
            Computerpictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Computer/" + dataTable.Rows[i].ItemArray[5].ToString() + "/" + dataTable.Rows[i].ItemArray[1].ToString());
                Computerpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Computerpictureboxes[i].Name = "Computer" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                Computerpictureboxes[i].Click += new EventHandler(this.Computer_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Computerpictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private void Show_AllStories_Panels()
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetAllStories(); if (dataTable == null) return;
            int Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count;
            Panel[] Panels = StoryPanels = new Panel[Num_OF_Panels];
            Storypictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Story/" + dataTable.Rows[i].ItemArray[4].ToString() + "/" + dataTable.Rows[i].ItemArray[1].ToString());
                Storypictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Storypictureboxes[i].Name = "Story" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[4].ToString();
                Storypictureboxes[i].Click += new EventHandler(this.Story_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Storypictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private void ShowAllItems()
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetAllStories();
            int Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count;
            Panel[] Panels = StoryPanels = new Panel[Num_OF_Panels];
            Storypictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Story/" + dataTable.Rows[i].ItemArray[4].ToString() + "/" + dataTable.Rows[i].ItemArray[1].ToString());
                Storypictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Storypictureboxes[i].Name = "Story" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[4].ToString();
                Storypictureboxes[i].Click += new EventHandler(this.Story_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Storypictureboxes[i]);
                Controls.Add(Panels[i]);
            }
            dataTable = Controller_OBJ.GetAllBooks(); if (dataTable == null) return;
            Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count;
            Panels = BookPanels = new Panel[Num_OF_Panels];
            Bookpictureboxes = new PictureBox[Num_OF_Panels];
            Labels = new LinkLabel[Num_OF_Panels];
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Book/" + dataTable.Rows[i].ItemArray[12].ToString() + "/" + dataTable.Rows[i].ItemArray[6].ToString());
                Bookpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[14]);
                Bookpictureboxes[i].Name = "Book" + "/" + dataTable.Rows[i].ItemArray[3].ToString() + "/" + dataTable.Rows[i].ItemArray[12].ToString();
                Bookpictureboxes[i].Click += new EventHandler(this.Book_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Bookpictureboxes[i]);
                Controls.Add(Panels[i]);
            }
            dataTable = Controller_OBJ.GetAllMusicInstruments(); if (dataTable == null) return;
            Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count;
            Panels = InstrumentPanels = new Panel[Num_OF_Panels];
            Instrumentspictureboxes = new PictureBox[Num_OF_Panels];
            Labels = new LinkLabel[Num_OF_Panels];
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Music Instrument/" + dataTable.Rows[i].ItemArray[5].ToString() + "/" + dataTable.Rows[i].ItemArray[1].ToString());
                Instrumentspictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Instrumentspictureboxes[i].Name = "Music Instrument" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                Instrumentspictureboxes[i].Click += new EventHandler(this.Instrument_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Instrumentspictureboxes[i]);
                Controls.Add(Panels[i]);
            }
            dataTable = Controller_OBJ.GetAllComputers(); if (dataTable == null) return;
            Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count;
            Panels = ComputerPanels = new Panel[Num_OF_Panels];
            Computerpictureboxes = new PictureBox[Num_OF_Panels];
            Labels = new LinkLabel[Num_OF_Panels];
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Computer/" + dataTable.Rows[i].ItemArray[5].ToString() + "/" + dataTable.Rows[i].ItemArray[1].ToString());
                Computerpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Computerpictureboxes[i].Name = "Computer" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                Computerpictureboxes[i].Click += new EventHandler(this.Computer_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Computerpictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private void Show_MyBooks_Panels()
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetMyBooks(User_ID); if (dataTable == null) return;
            int Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count;
            Panel[] Panels = BookPanels = new Panel[Num_OF_Panels];
            Bookpictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Book/" + dataTable.Rows[i].ItemArray[12].ToString() + "/" + dataTable.Rows[i].ItemArray[6].ToString());
                Bookpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[14]);
                Bookpictureboxes[i].Name = "Book" + "/" + dataTable.Rows[i].ItemArray[3].ToString() + "/" + dataTable.Rows[i].ItemArray[12].ToString();
                Bookpictureboxes[i].Click += new EventHandler(this.Book_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Bookpictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private void Show_MyStories_Panels()
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetMyStories(User_ID); if (dataTable == null) return;
            int Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count;
            Panel[] Panels = StoryPanels = new Panel[Num_OF_Panels];
            Storypictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Story/" + dataTable.Rows[i].ItemArray[4].ToString() + "/" + dataTable.Rows[i].ItemArray[1].ToString());
                Storypictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Storypictureboxes[i].Name = "Story" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[4].ToString();
                Storypictureboxes[i].Click += new EventHandler(this.Story_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Storypictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private void Show_MyInstruments_Panels()
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetMyInstruments(User_ID); if (dataTable == null) return;
            int Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count;
            Panel[] Panels = InstrumentPanels = new Panel[Num_OF_Panels];
            Instrumentspictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Music Instrument/" + dataTable.Rows[i].ItemArray[5].ToString() + "/" + dataTable.Rows[i].ItemArray[1].ToString());
                Instrumentspictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Instrumentspictureboxes[i].Name = "Music Instrument" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                Instrumentspictureboxes[i].Click += new EventHandler(this.Instrument_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Instrumentspictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private void Show_MyComputers_Panels()
        {
            ActivePanel.Visible = false;
            dataTable = Controller_OBJ.GetMyComputers(User_ID); if (dataTable == null) return;
            int Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count;
            Panel[] Panels = ComputerPanels = new Panel[Num_OF_Panels];
            Computerpictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                Labels[i] = CreateLabel(i, "Computer/" + dataTable.Rows[i].ItemArray[5].ToString() + "/" + dataTable.Rows[i].ItemArray[1].ToString());
                Computerpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                Computerpictureboxes[i].Name = "Computer" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                Computerpictureboxes[i].Click += new EventHandler(this.Computer_PictureBox_Click);
                InesertControlToPanel(ref Panels[i], ref Labels[i], ref Computerpictureboxes[i]);
                Controls.Add(Panels[i]);
            }
        }
        private void Show_Filtered_Panels(string Filteration_Type, string Type)
        {
            //Music Instrument
            ActivePanel.Visible = false;
            int Num_OF_Panels = 0;
            if (Filteration_Type == "TopRated" && Type == "")
            {
                if (CurrentPanel == "Book") { dataTable = Controller_OBJ.GetTopRatedBooks(); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story") { dataTable = Controller_OBJ.GetTopRatedStories(); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument") { dataTable = Controller_OBJ.GetTopRatedMusical_Instrument(); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer") { dataTable = Controller_OBJ.GetTopRatedComputer(); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }
            else if (Filteration_Type == "LowRated" && Type == "")
            {
                if (CurrentPanel == "Book") { dataTable = Controller_OBJ.GetLowRatedBooks(); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story") { dataTable = Controller_OBJ.GetLowRatedStories(); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument") { dataTable = Controller_OBJ.GetLowRatedMusical_Instrument(); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer") { dataTable = Controller_OBJ.GetLowRatedComputer(); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }
            else if (Filteration_Type == "TopPrice" && Type == "")
            {
                if (CurrentPanel == "Book") { dataTable = Controller_OBJ.GetTopPriceBooks(); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story") { dataTable = Controller_OBJ.GetTopPriceStories(); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument") { dataTable = Controller_OBJ.GetTopPriceMusical_Instrument(); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer") { dataTable = Controller_OBJ.GetTopPriceComputer(); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }
            else if (Filteration_Type == "LowPrice" && Type == "")
            {
                if (CurrentPanel == "Book") { dataTable = Controller_OBJ.GetLowPriceBooks(); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story") { dataTable = Controller_OBJ.GetLowPriceStories(); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument") { dataTable = Controller_OBJ.GetLowPriceMusical_Instrument(); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer") { dataTable = Controller_OBJ.GetLowPriceComputer(); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }
            else if (Filteration_Type == "Available" && Type == "")
            {
                if (CurrentPanel == "Book") { dataTable = Controller_OBJ.GetAvailableBooks(); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story") { dataTable = Controller_OBJ.GetAvailableStories(); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument") { dataTable = Controller_OBJ.GetAvailableMusical_Instrument(); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer") { dataTable = Controller_OBJ.GetAvailableComputer(); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }
            else if (Filteration_Type == "NotAvailable" && Type == "")
            {
                if (CurrentPanel == "Book") { dataTable = Controller_OBJ.GetNotAvailableBooks(); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story") { dataTable = Controller_OBJ.GetNotAvailableStories(); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument") { dataTable = Controller_OBJ.GetNotAvailableMusical_Instrument(); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer") { dataTable = Controller_OBJ.GetNotAvailableComputer(); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }
            //------------------Specific Filtering--------------------------------//
            else if (Filteration_Type == "TopRated" && Type != "")
            {
                if (CurrentPanel == "Book" + '/' + Type) { dataTable = Controller_OBJ.GetTopRatedBooksBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story" + '/' + Type) { dataTable = Controller_OBJ.GetTopRatedStoriesBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument" + '/' + Type) { dataTable = Controller_OBJ.GetTopRatedMusical_InstrumentBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer" + '/' + Type) { dataTable = Controller_OBJ.GetTopRatedComputerBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }
            else if (Filteration_Type == "LowRated" && Type != "")
            {
                if (CurrentPanel == "Book" + '/' + Type) { dataTable = Controller_OBJ.GetLowRatedBooksBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story" + '/' + Type) { dataTable = Controller_OBJ.GetLowRatedStoriesBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument" + '/' + Type) { dataTable = Controller_OBJ.GetLowRatedMusical_InstrumentBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer" + '/' + Type) { dataTable = Controller_OBJ.GetLowRatedComputerBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }
            else if (Filteration_Type == "TopPrice" && Type != "")
            {
                if (CurrentPanel == "Book" + '/' + Type) { dataTable = Controller_OBJ.GetTopPriceBooksBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story" + '/' + Type) { dataTable = Controller_OBJ.GetTopPriceStoriesBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument" + '/' + Type) { dataTable = Controller_OBJ.GetTopPriceMusical_InstrumentBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer" + '/' + Type) { dataTable = Controller_OBJ.GetTopPriceComputerBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }
            else if (Filteration_Type == "LowPrice" && Type != "")
            {
                if (CurrentPanel == "Book" + '/' + Type) { dataTable = Controller_OBJ.GetLowPriceBooksBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story" + '/' + Type) { dataTable = Controller_OBJ.GetLowPriceStoriesBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument" + '/' + Type) { dataTable = Controller_OBJ.GetLowPriceMusical_InstrumentBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer" + '/' + Type) { dataTable = Controller_OBJ.GetLowPriceComputerBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }
            else if (Filteration_Type == "Available" && Type != "")
            {
                if (CurrentPanel == "Book" + '/' + Type) { dataTable = Controller_OBJ.GetAvailableBookBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story" + '/' + Type) { dataTable = Controller_OBJ.GetAvailableStoriesBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument" + '/' + Type) { dataTable = Controller_OBJ.GetAvailableMusical_InstrumentBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer" + '/' + Type) { dataTable = Controller_OBJ.GetAvailableComputerBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }
            else if (Filteration_Type == "NotAvailable" && Type != "")
            {
                if (CurrentPanel == "Book" + '/' + Type) { dataTable = Controller_OBJ.GetNotAvailableBooksBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofBookpictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Story" + '/' + Type) { dataTable = Controller_OBJ.GetNotAvailableStoriesBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofStorypictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Music Instrument" + '/' + Type) { dataTable = Controller_OBJ.GetNotAvailableMusical_InstrumentBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofInstrumentspictureboxes = dataTable.Rows.Count; }
                else if (CurrentPanel == "Computer" + '/' + Type) { dataTable = Controller_OBJ.GetNotAvailableComputerBySection(Type); if (dataTable == null) return; Num_OF_Panels = NumofComputerpictureboxes = dataTable.Rows.Count; }
            }


            Panel[] Panels = BookPanels = new Panel[Num_OF_Panels];
            Bookpictureboxes = new PictureBox[Num_OF_Panels];
            LinkLabel[] Labels = new LinkLabel[Num_OF_Panels];
            int incrementer = 1, rows = 1;
            for (int i = 0; i < Num_OF_Panels; ++i)
            {
                if (incrementer == 5) { rows++; incrementer = 1; }
                Panels[i] = CreatePanel(i, Color.White, 214, 260, 225 * (incrementer++), 75 + (rows - 1) * 300);
                if (CurrentPanel == "Book")
                {
                    Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[6].ToString());
                    Bookpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[14]);
                    Bookpictureboxes[i].Name = Labels[i].Text + "/" + dataTable.Rows[i].ItemArray[3].ToString() + "/" + dataTable.Rows[i].ItemArray[12].ToString();
                    Bookpictureboxes[i].Click += new EventHandler(this.Book_PictureBox_Click);
                    InesertControlToPanel(ref Panels[i], ref Labels[i], ref Bookpictureboxes[i]);
                    Controls.Add(Panels[i]);
                }
                else if (CurrentPanel == "Story")
                {
                    Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[1].ToString());
                    Storypictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                    Storypictureboxes[i].Name = "Stories" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[4].ToString();
                    Storypictureboxes[i].Click += new System.EventHandler(this.Story_PictureBox_Click);
                    InesertControlToPanel(ref Panels[i], ref Labels[i], ref Storypictureboxes[i]);
                }
                else if (CurrentPanel == "Music Instrument")
                {
                    Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[1].ToString());
                    Instrumentspictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                    Instrumentspictureboxes[i].Name = "Instruments" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                    Instrumentspictureboxes[i].Click += new System.EventHandler(this.Instrument_PictureBox_Click);
                    InesertControlToPanel(ref Panels[i], ref Labels[i], ref Instrumentspictureboxes[i]);
                }
                else if (CurrentPanel == "Computer")
                {
                    Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[5].ToString());
                    Computerpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                    Computerpictureboxes[i].Name = "Computers" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                    Computerpictureboxes[i].Click += new System.EventHandler(this.Computer_PictureBox_Click);
                    InesertControlToPanel(ref Panels[i], ref Labels[i], ref Computerpictureboxes[i]);
                }
                else if (CurrentPanel == "Book" + '/' + Type)
                {
                    Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[6].ToString());
                    Bookpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[14]);
                    Bookpictureboxes[i].Name = Labels[i].Text + "/" + dataTable.Rows[i].ItemArray[3].ToString() + "/" + dataTable.Rows[i].ItemArray[12].ToString();
                    Bookpictureboxes[i].Click += new EventHandler(this.Book_PictureBox_Click);
                    InesertControlToPanel(ref Panels[i], ref Labels[i], ref Bookpictureboxes[i]);
                    Controls.Add(Panels[i]);
                }
                else if (CurrentPanel == "Story" + '/' + Type)
                {
                    Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[1].ToString());
                    Storypictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                    Storypictureboxes[i].Name = "Stories" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[4].ToString();
                    Storypictureboxes[i].Click += new System.EventHandler(this.Story_PictureBox_Click);
                    InesertControlToPanel(ref Panels[i], ref Labels[i], ref Storypictureboxes[i]);
                }
                else if (CurrentPanel == "Music Instrument" + '/' + Type)
                {
                    Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[1].ToString());
                    Instrumentspictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                    Instrumentspictureboxes[i].Name = "Instruments" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                    Instrumentspictureboxes[i].Click += new System.EventHandler(this.Instrument_PictureBox_Click);
                    InesertControlToPanel(ref Panels[i], ref Labels[i], ref Instrumentspictureboxes[i]);
                }
                else if (CurrentPanel == "Computer" + '/' + Type)
                {
                    Labels[i] = CreateLabel(i, dataTable.Rows[i].ItemArray[5].ToString());
                    Computerpictureboxes[i] = CreatePicturebox(i, (byte[])dataTable.Rows[i].ItemArray[7]);
                    Computerpictureboxes[i].Name = "Computers" + "/" + dataTable.Rows[i].ItemArray[0].ToString() + "/" + dataTable.Rows[i].ItemArray[5].ToString();
                    Computerpictureboxes[i].Click += new System.EventHandler(this.Computer_PictureBox_Click);
                    InesertControlToPanel(ref Panels[i], ref Labels[i], ref Computerpictureboxes[i]);
                }

                Controls.Add(Panels[i]);
            }
        }


    }
}
