using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CubeStudio

{
    public partial class SelectBodyPartType : Form
    {
        public RadioButton selectedButton;

        public SelectBodyPartType()
        {
            InitializeComponent();

            cancel.Click += new EventHandler(cancelEvent);
            addToModel.Click += new EventHandler(addPartEvent);
            foreach (var tab in tabControl1.Controls)
            {
                foreach (var control in ((TabPage)tab).Controls)
                {
                    RadioButton radio = control as RadioButton;

                    if (radio != null)
                    {
                        radio.Click += new EventHandler(boxChecked);
                    }
                }
            }
            this.radioButton1.Name = "Humanoid Head";
            this.radioButton10.Name = "Humanoid Right Lower Leg";
            this.radioButton9.Name = "Humanoid Right Leg";
            this.radioButton8.Name = "Humanoid Left Lower Leg";
            this.radioButton7.Name = "Humanoid Left Leg";
            this.radioButton6.Name = "Humanoid Right Lower Arm";
            this.radioButton5.Name = "Humanoid Right Arm";
            this.radioButton4.Name = "Humanoid Left Lower Arm";
            this.radioButton3.Name = "Humanoid Left Arm";
            this.radioButton2.Name = "Humanoid Body";
            this.radioButton11.Name = "Wheel";
            this.radioButton12.Name = "Rigid Connection";
        }



        private void boxChecked(object sender, EventArgs e)
        {
            if (sender is RadioButton)
            {
                selectedButton = (RadioButton)sender;
                //MessageBox.Show(selectedButton.Text);
                //Code to use radioButton's properties to do something useful.
            }
        }

        private void cancelEvent(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void SelectBodyPartType_FormClosed(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void addPartEvent(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
