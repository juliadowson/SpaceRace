/*Julia Dowson
 * Mr. T
 * March 23, 2021
 * This is a basic two player game based off of the old aracade game, Space Race.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace SpaceRace
{
    public partial class p2ScoreOutput : Form
    {
        int player1X = 50;
        int player1Y = 340;
        int p1Score = 0;
        int p2Score = 0;
        int player2X = 250;
        int player2Y = 340;
        int playerWidth = 15;
        int playerHeight = 15;
        int playerSpeed = 8;

        int obHeight = 7;
        int obLength = 10;

        string winner;

        bool upArrowDown = false;
        bool downArrowDown = false;
        bool sDown = false;
        bool wDown = false;

        List<int> leftYScrollList = new List<int>();
        List<int> leftXScrollList = new List<int>();
        List<int> rightYScrollList = new List<int>();
        List<int> rightXScrollList = new List<int>();
        List<int> obSpeedList = new List<int>();

        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        SoundPlayer beep = new SoundPlayer(Properties.Resources.beep);
        SoundPlayer gameOverSound = new SoundPlayer(Properties.Resources.gameOver);
        SoundPlayer pointSound = new SoundPlayer(Properties.Resources.point);
        SoundPlayer openingSound = new SoundPlayer(Properties.Resources.opening);

        Random randGen = new Random();
        int randValue = 0;

        string gameState = "waiting";

        public p2ScoreOutput()
        {
            InitializeComponent();
            leftYScrollList.Add(0);
            leftXScrollList.Add(0);
            rightYScrollList.Add(0);
            rightXScrollList.Add(0);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "gameOver")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "gameOver")
                    {
                        Application.Exit();
                    }
                    break;
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;

            }
        }

        //what happens when the space bar is pressed
        public void GameInitialize()
        {
            openingSound.Play();
            titleLabel.Text = "";
            subTitleLabel.Text = "";
            p1ScoreOutput.Text = "0";
            p2ScoreOuput.Text = "0";

            gameTimer.Enabled = true;
            gameState = "running";

            leftYScrollList.Clear();
            leftXScrollList.Clear();
            rightYScrollList.Clear();
            rightXScrollList.Clear();
            obSpeedList.Clear();

            p1Score = 0;
            p2Score = 0;
            player1X = 170;
            player1Y = 340;
            player2X = 400;
            player2Y = 340;
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            Rectangle player1Rec = new Rectangle(player1X, player1Y, playerWidth, playerHeight);
            Rectangle player2Rec = new Rectangle(player2X, player2Y, playerWidth, playerHeight);

            #region move
            if (upArrowDown == true && player2Y > 0)
            {
                player2Y -= playerSpeed;
            }

            if (downArrowDown == true && player2Y < this.Height - playerHeight)
            {
                player2Y += playerSpeed;
            }

            if (wDown == true && player1Y > 0)
            {
                player1Y -= playerSpeed;
            }

            if (sDown == true && player1Y < this.Height - playerHeight)
            {
                player1Y += playerSpeed;
            }
            #endregion

            randValue = randGen.Next(0, 101);

            //obstacles are created at random points on either side of the screen
            if (randValue < 6)
            {
                leftXScrollList.Add(10);
                leftYScrollList.Add(randGen.Next(10, this.Height - 50));
                obSpeedList.Add(randGen.Next(2, 6));
            }
            else if (randValue < 12)
            {
                rightXScrollList.Add(600);
                rightYScrollList.Add(randGen.Next(10, this.Height - 50));
                obSpeedList.Add(randGen.Next(2, 5));
            }

            //moves the obstacles
            for (int i = 0; i < leftXScrollList.Count(); i++)
            { 
                leftXScrollList[i] += obSpeedList[i];
            }

            for (int i = 0; i < rightXScrollList.Count(); i++)
            {
                rightXScrollList[i] -= obSpeedList[i];
            }

            for (int i = 0; i < leftXScrollList.Count(); i++)
            {
                if (leftXScrollList[i] > 600)
                {
                    leftXScrollList.RemoveAt(i);
                    leftYScrollList.RemoveAt(i);
                    obSpeedList.RemoveAt(i);
                }
            }

            for (int i = 0; i < rightXScrollList.Count(); i++)
            {
                if (rightXScrollList[i] < 0)
                {
                    rightXScrollList.RemoveAt(i);
                    rightYScrollList.RemoveAt(i);
                    obSpeedList.RemoveAt(i);
                }
            }

            //what happens if an obstacle hits a player
            for (int i = 0; i < leftXScrollList.Count(); i++)
            {
                Rectangle obRec = new Rectangle(leftXScrollList[i], leftYScrollList[i], obLength, obHeight);
                leftXScrollList[i] += obSpeedList[i];

                if (player1Rec.IntersectsWith(obRec))
                {
                    beep.Play();
                    player1X = 170;
                    player1Y = 340;
                }
                else if (player2Rec.IntersectsWith(obRec))
                {
                    beep.Play();
                    player2X = 400;
                    player2Y = 340; 
                }
            }

            for (int i = 0; i < rightXScrollList.Count(); i++)
            {
                Rectangle ob2Rec = new Rectangle(rightXScrollList[i], rightYScrollList[i], obLength, obHeight);
                rightXScrollList[i] -= obSpeedList[i];
                if (player1Rec.IntersectsWith(ob2Rec))
                {
                    beep.Play();
                    player1X = 170;
                    player1Y = 340;
                }
                else if (player2Rec.IntersectsWith(ob2Rec))
                {
                    beep.Play();
                    player2X = 400;
                    player2Y = 340;
                }
            }

            //if the either player gets to the top of the screen
            if (player1Y <= 3)
            {
                pointSound.Play();
                p1Score++;
                player1X = 170;
                player1Y = 340;
                p1ScoreOutput.Text = $"{p1Score}";
            }
            else if (player2Y <= 3)
            {
                pointSound.Play();
                p2Score++;
                player2X = 400;
                player2Y = 340;
                p2ScoreOuput.Text = $"{p2Score}";
            }
            
            if (p1Score == 3 )
            {
                gameState = "gameOver";
                winner = "Player 1 Wins!";
            }
            else if (p2Score == 3)
            {
                gameState = "gameOver";
                winner = "Player 2 Wins!";
            }
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                titleLabel.Text = "Dodge Runner";
                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
            }
            else if (gameState == "running")
            {
                e.Graphics.FillRectangle(redBrush, player1X, player1Y, playerWidth, playerHeight);
                e.Graphics.FillRectangle(redBrush, player2X, player2Y, playerWidth, playerHeight);

                for (int i = 0; i < leftYScrollList.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, leftXScrollList[i], leftYScrollList[i], obLength, obHeight);
                }

                for (int i = 0; i < rightYScrollList.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, rightXScrollList[i], rightYScrollList[i], obLength, obHeight);
                }
            }

            //what happens if either player reaches 3 points
            else if (gameState == "gameOver")
            {
                gameOverSound.Play();
                titleLabel.Text = "GAME OVER";
                subTitleLabel.Text = $"{winner} \n Press Space to Start, Escape to Exit";
                p1ScoreOutput.Text = "";
                p2ScoreOuput.Text = "";
                gameTimer.Enabled = false;
            }
        }
    }
}
