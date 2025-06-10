using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        // firstClicked points to the first Label control that the player clicks,
        // but it will be null if the player hasn't clicked a label yet
        Label firstClicked = null;
        // secondClicked points to the second Label control that the player clicks,
        Label secondClicked = null;

        // Use this Random object to choose random icons for the squares
        Random random = new Random();
        // Each of these letters is an interesting icon in the Webdings font,
        // and each icon appears twice in this list
        List<string> icons = new List<string>()
        {"!","!","N","N",",",",","k","k","b","b","v","v","w","w","z","z"};
        private void AssignIconsToSquares()
        {
            // This method assigns icons to the squares in the game
            // It randomly selects icons from the icons list and assigns them to each square
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconlable = control as Label;
                if (iconlable != null)
                {
                    int randomIndex = random.Next(icons.Count);
                    iconlable.Text = icons[randomIndex];
                    iconlable.ForeColor = iconlable.BackColor; // Hide the icon by matching the text color with the background color
                    icons.RemoveAt(randomIndex); // Remove the icon so it can't be used again
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares(); // Call the method to assign icons when the form loads
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
            // The timer is only on after two non-matching icons have been shown to the player, so ignore any clicks if the timer is running
            if (timer1.Enabled)
            {
                return; // If the timer is running, do nothing
            }
            Label clickedlable = sender as Label;
            if (clickedlable != null)
            {
                if (clickedlable.ForeColor == Color.Black)
                {
                    return; // If the icon is already visible, do nothing
                }
                if (firstClicked == null) // If this is the first label clicked
                {
                    firstClicked = clickedlable;
                    firstClicked.ForeColor = Color.Black; // Show the icon
                    return; // Exit the method
                }
                // If the player gets this far, the timer isn't running and firstClicked isn't null, so this must be the second icon the player clicked Set its color to black
                secondClicked = clickedlable;
                secondClicked.ForeColor = Color.Black;
                CheckForWinner(); // Check if the player has won before allowing any more click.       

                // If the player gets this far, the player clicked two different icons, so start the timer (which will wait three quarters of a second, and then hide the icons)
                if (secondClicked.Text == firstClicked.Text)
                {
                    // If the player clicked two matching icons, keep them visible
                    firstClicked = null;
                    secondClicked = null;
                    return; // Exit the method
                }
                // If the player gets this far, the player clicked two different icons, so start the timer (which will wait three quarters of a second, and then hide the icons)
                timer1.Start(); // Start the timer to hide the icons after a delay

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //stop the timer
            timer1.Stop();

            // Hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Reset firstClicked and secondClicked so the next time a label is clicked, the program knows it's the first click
            firstClicked = null;
            secondClicked = null;

        }
        private void CheckForWinner()
        {
            // Go through all of the labels in the TableLayoutPanel, checking each one to see if its icon is matched
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconlable = control as Label;
                if (iconlable != null)
                {
                    // If any label's icon is still hidden, the player hasn't won yet
                    if (iconlable.ForeColor == iconlable.BackColor)
                    {
                        return; // Exit the method
                    }
                }
            }
            //If the loop didn’t return, it didn't find any unmatched icons That means the user won. Show a message and close the form
            MessageBox.Show("You matched all the icons!", "Congratulations"); Close();
        }
    }
}
