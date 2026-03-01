using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClickGame
{
    public class GameForm : Form
    {
        private Button target;
        private Label scoreLabel;
        private Label timeLabel;
        private System.Windows.Forms.Timer moveTimer;
        private System.Windows.Forms.Timer gameTimer;

        private int score = 0;
        private int timeLeft = 30;
        private readonly Random random = new();

        public GameForm()
        {
            Text = "Click The Box";
            Size = new Size(600, 400);

            scoreLabel = new Label()
            {
                Text = "Score: 0",
                Location = new Point(10, 10),
                AutoSize = true
            };

            timeLabel = new Label()
            {
                Text = "Time: 30",
                Location = new Point(500, 10),
                AutoSize = true
            };

            target = new Button()
            {
                Size = new Size(60, 60),
                BackColor = Color.Red
            };

            target.Click += (s, e) =>
            {
                score++;
                scoreLabel.Text = $"Score: {score}";
                MoveTarget();
            };

            moveTimer = new System.Windows.Forms.Timer
            {
                Interval = 800
            };
            moveTimer.Tick += (s, e) => MoveTarget();
            moveTimer.Start();

            gameTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            gameTimer.Tick += (s, e) =>
            {
                timeLeft--;
                timeLabel.Text = $"Time: {timeLeft}";

                if (timeLeft <= 0)
                {
                    moveTimer.Stop();
                    gameTimer.Stop();
                    target.Enabled = false;
                    MessageBox.Show($"Game Over!\nYour Score: {score}");
                }
            };
            gameTimer.Start();

            Controls.Add(scoreLabel);
            Controls.Add(timeLabel);
            Controls.Add(target);

            MoveTarget();
        }

        private void MoveTarget()
        {
            int x = random.Next(0, ClientSize.Width - target.Width);
            int y = random.Next(50, ClientSize.Height - target.Height);
            target.Location = new Point(x, y);
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new GameForm());
        }
    }
}