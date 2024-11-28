using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace Calculator
{
    public partial class Calculator : Form
    {

        private Rectangle oneButtonOriginalRectangle;
        private Rectangle twoButtonOriginalRectangle;
        private Rectangle treeButtonOriginalRectangle;
        private Rectangle fourButtonOriginalRectangle;
        private Rectangle fiveButtonOriginalRectangle;
        private Rectangle sixButtonOriginalRectangle;
        private Rectangle sevenButtonOriginalRectangle;
        private Rectangle eightButtonOriginalRectangle;
        private Rectangle nineButtonOriginalRectangle;
        private Rectangle zeroButtonOriginalRectangle;
        private Rectangle backButtonOriginalRectangle;
        private Rectangle plusminusButtonOriginalRectangle;
        private Rectangle clearButtonOriginalRectangle;
        private Rectangle decimalButtonOriginalRectangle;
        private Rectangle squareButtonOriginalRectangle;
        private Rectangle myltiplyButtonOriginalRectangle;
        private Rectangle divideButtonOriginalRectangle;
        private Rectangle minusButtonOriginalRectangle;
        private Rectangle equalButtonOriginalRectangle;
        private Rectangle plusButtonOriginalRectangle;
        private Rectangle displayLableButtonOriginalRectangle;
        private Size originalFormSize;



        public Calculator()
        {
            InitializeComponent();
        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            originalFormSize = this.Size;
            oneButtonOriginalRectangle = new Rectangle(oneButton.Location.X, oneButton.Location.Y, oneButton.Width, oneButton.Height);
            twoButtonOriginalRectangle = new Rectangle(twoButton.Location.X, twoButton.Location.Y, twoButton.Width, twoButton.Height);
            treeButtonOriginalRectangle = new Rectangle(threeButton.Location.X, threeButton.Location.Y, threeButton.Width, threeButton.Height);
            fourButtonOriginalRectangle = new Rectangle(fourButton.Location.X, fourButton.Location.Y, fourButton.Width, fourButton.Height);
            fiveButtonOriginalRectangle = new Rectangle(fiveButton.Location.X, fiveButton.Location.Y, fiveButton.Width, fiveButton.Height);
            sixButtonOriginalRectangle = new Rectangle(sixButton.Location.X, sixButton.Location.Y, sixButton.Width, sixButton.Height);
            sevenButtonOriginalRectangle = new Rectangle(severnButton.Location.X, severnButton.Location.Y, severnButton.Width, severnButton.Height);
            eightButtonOriginalRectangle = new Rectangle(eightButton.Location.X, eightButton.Location.Y, eightButton.Width, eightButton.Height);
            nineButtonOriginalRectangle = new Rectangle(nineButton.Location.X, nineButton.Location.Y, nineButton.Width, nineButton.Height);
            zeroButtonOriginalRectangle = new Rectangle(zerooButton.Location.X, zerooButton.Location.Y, zerooButton.Width, zerooButton.Height);
            backButtonOriginalRectangle = new Rectangle(backButton.Location.X, backButton.Location.Y, backButton.Width, backButton.Height);
            plusminusButtonOriginalRectangle = new Rectangle(plusminusButton.Location.X, plusminusButton.Location.Y, plusminusButton.Width, plusminusButton.Height);
            clearButtonOriginalRectangle = new Rectangle(clearButton.Location.X, clearButton.Location.Y, clearButton.Width, clearButton.Height);
            decimalButtonOriginalRectangle = new Rectangle(decimalButton.Location.X, decimalButton.Location.Y, decimalButton.Width, decimalButton.Height);
            squareButtonOriginalRectangle = new Rectangle(squeryButton.Location.X, squeryButton.Location.Y, squeryButton.Width, squeryButton.Height);
            myltiplyButtonOriginalRectangle = new Rectangle(myltipyButton.Location.X, myltipyButton.Location.Y, myltipyButton.Width, myltipyButton.Height);
            divideButtonOriginalRectangle = new Rectangle(divideButton.Location.X, divideButton.Location.Y, divideButton.Width, divideButton.Height);
            minusButtonOriginalRectangle = new Rectangle(minusButton.Location.X, minusButton.Location.Y, minusButton.Width, minusButton.Height);
            equalButtonOriginalRectangle = new Rectangle(equalButton.Location.X, equalButton.Location.Y, equalButton.Width, equalButton.Height);
            plusButtonOriginalRectangle = new Rectangle(plusButton.Location.X, plusButton.Location.Y, plusButton.Width, plusButton.Height);
            displayLableButtonOriginalRectangle = new Rectangle(displayLable.Location.X, displayLable.Location.Y, displayLable.Width, displayLable.Height);
        }

        private void Calculator_Resize(object sender, EventArgs e)
        {
            resizeControl(oneButtonOriginalRectangle, oneButton);
            resizeControl(twoButtonOriginalRectangle, twoButton);
            resizeControl(treeButtonOriginalRectangle, threeButton);
            resizeControl(fourButtonOriginalRectangle, fourButton);
            resizeControl(fiveButtonOriginalRectangle, fiveButton);
            resizeControl(sixButtonOriginalRectangle, sixButton);
            resizeControl(sevenButtonOriginalRectangle, severnButton);
            resizeControl(eightButtonOriginalRectangle, eightButton);
            resizeControl(nineButtonOriginalRectangle, nineButton);
            resizeControl(zeroButtonOriginalRectangle, zerooButton);
            resizeControl(backButtonOriginalRectangle, backButton);
            resizeControl(plusminusButtonOriginalRectangle, plusminusButton);
            resizeControl(clearButtonOriginalRectangle, clearButton);
            resizeControl(decimalButtonOriginalRectangle, decimalButton);
            resizeControl(squareButtonOriginalRectangle, squeryButton);
            resizeControl(myltiplyButtonOriginalRectangle, myltipyButton);
            resizeControl(divideButtonOriginalRectangle, divideButton);
            resizeControl(minusButtonOriginalRectangle, minusButton);
            resizeControl(equalButtonOriginalRectangle, equalButton);
            resizeControl(plusButtonOriginalRectangle, plusButton);
            resizeControl(displayLableButtonOriginalRectangle, displayLable);

        }


        private void resizeControl(Rectangle originalControlRect, Control control)
        {
            float xAxis = (float)(this.Width) / (float)(originalFormSize.Width);
            float yAxis = (float)(this.Height) / (float)(originalFormSize.Height);

            int newXPosition = (int)(originalControlRect.X * xAxis);
            int newYPosition = (int)(originalControlRect.Y * yAxis);

            int newWith = (int)(originalControlRect.Width * xAxis);
            int newHight = (int)(originalControlRect.Height * yAxis);

            control.Location = new Point(newXPosition, newYPosition);
            control.Size = new Size(newWith, newHight);


        }

        float num1, num2, result;
        char operation;
        bool dec = false;

        private void changeLable(int numPressed)
        {
            if (dec == true)
            {
                int decimalCount = 0;
                foreach (char c in displayLable.Text)
                {
                    if (c == '.')
                    {
                        decimalCount++;
                    }
                }
                if (decimalCount < 1)
                {
                    displayLable.Text = displayLable.Text + ".";
                }
                dec = false;
            }
            else
            {
                if (displayLable.Text.Equals("0") == true && displayLable.Text != null)
                {
                    displayLable.Text = numPressed.ToString();
                }
                else if (displayLable.Text.Equals("-0") == true)
                {
                    displayLable.Text = "-" + numPressed.ToString();
                }
                else
                {
                    displayLable.Text = displayLable.Text + numPressed.ToString();
                }
            }
        }

        private void zerooButton_Click(object sender, EventArgs e)
        {
            changeLable(0);
        }

        private void oneButton_Click(object sender, EventArgs e)
        {
            changeLable(1);
        }

        private void twoButton_Click(object sender, EventArgs e)
        {
            changeLable(2);
        }

        private void threeButton_Click(object sender, EventArgs e)
        {
            changeLable(3);
        }

        private void fourButton_Click(object sender, EventArgs e)
        {
            changeLable(4);
        }

        private void fiveButton_Click(object sender, EventArgs e)
        {
            changeLable(5);

        }

        private void sixButton_Click(object sender, EventArgs e)
        {
            changeLable(6);

        }

        private void severnButton_Click(object sender, EventArgs e)
        {
            changeLable(7);

        }

        private void eightButton_Click(object sender, EventArgs e)
        {
            changeLable(8);

        }

        private void nineButton_Click(object sender, EventArgs e)
        {
            changeLable(9);

        }

        private void decimalButton_Click(object sender, EventArgs e)
        {
            dec = true;
            changeLable(0);

        }

        private void plusminusButton_Click(object sender, EventArgs e)
        {
            displayLable.Text = "-" + displayLable.Text;
        }

        private void squeryButton_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(displayLable.Text))
            {
                MessageBox.Show("Please enter a valid number.");
                return;
            }
            if (float.TryParse(displayLable.Text, out num1))
            {
                if (num1 > 0)
                {
                    double sqrt = Math.Sqrt(num1);
                    displayLable.Text = sqrt.ToString();
                }
                else
                {
                    MessageBox.Show("Please enter a positive number.");
                }
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }
        private void clearButton_Click(object sender, EventArgs e)
        {
            displayLable.Text = "0";
            num1 = 0;
            num2 = 0;
            result = 0;
            operation = '\0';
            dec = false;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            int stringLength = displayLable.Text.Length;
            if (stringLength > 1)
            {
                displayLable.Text = displayLable.Text.Substring(0, stringLength - 1);
            }
            else
            {
                displayLable.Text = "0";
            }
        }

        private void myltipyButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(displayLable.Text))
            {
                MessageBox.Show("Please enter a valid number.");
                return;
            }

            if (float.TryParse(displayLable.Text, out num1))
            {
                operation = '*';
                result = result * num1;
                displayLable.Text = "";
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }

        private void divideButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(displayLable.Text))
            {
                MessageBox.Show("Please enter a valid number.");
                return;
            }

            if (float.TryParse(displayLable.Text, out num1))
            {
                if (num1 == 0)
                {
                    MessageBox.Show("Cannot divide by zero.");
                    return;
                }

                operation = '/';
                result = result / num1;
                displayLable.Text = "";
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a valid number.");
            }
        }

        private void minusButton_Click(object sender, EventArgs e)
        {
            if (float.TryParse(displayLable.Text, out num1))
            {
                operation = '-';
                result -= num1;
                displayLable.Text = "";
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a valid number before performing subtraction.");
            }
        }

        private void plusButton_Click(object sender, EventArgs e)
        {
            if (float.TryParse(displayLable.Text, out num1))
            {
                operation = '+';
                result += num1;
                displayLable.Text = "";
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a valid number before performing addition.");
            }
        }

        private void equalButton_Click(object sender, EventArgs e)
        {
            result = 0;
            if (string.IsNullOrWhiteSpace(displayLable.Text))
            {
                MessageBox.Show("Please enter a valid number before pressing equals.");
                return;
            }

            switch (operation)
            {
                case '+':
                    if (float.TryParse(displayLable.Text, out num2))
                    {
                        result = num1 + num2;
                    }
                    else
                    {
                        MessageBox.Show("Invalid input. Please enter a valid number.");
                        return;
                    }
                    break;

                case '-':
                    if (float.TryParse(displayLable.Text, out num2))
                    {
                        result = num1 - num2;
                    }
                    else
                    {
                        MessageBox.Show("Invalid input. Please enter a valid number.");
                        return;
                    }
                    break;

                case '/':
                    if (float.TryParse(displayLable.Text, out num2))
                    {
                        if (num2 == 0)
                        {
                            MessageBox.Show("Cannot divide by zero.");
                            return;
                        }
                        result = num1 / num2;
                    }
                    else
                    {
                        MessageBox.Show("Invalid input. Please enter a valid number.");
                        return;
                    }
                    break;

                case '*':
                    if (float.TryParse(displayLable.Text, out num2))
                    {
                        result = num1 * num2;
                    }
                    else
                    {
                        MessageBox.Show("Invalid input. Please enter a valid number.");
                        return;
                    }
                    break;

                default:
                    MessageBox.Show("Invalid operation. Please try again.");
                    return;
            }

            
            displayLable.Text = result.ToString();
        }

        private void displayLable_Click(object sender, EventArgs e)
        {

        }

        private void Calculator_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.NumPad1:
                    oneButton.PerformClick();
                    break;
                case Keys.D1:
                    oneButton.PerformClick();
                    break;
                case Keys.NumPad2:
                    twoButton.PerformClick();
                    break;
                case Keys.D2:
                    twoButton.PerformClick();
                    break;
                case Keys.NumPad3:
                    threeButton.PerformClick();
                    break;
                case Keys.D3:
                    threeButton.PerformClick();
                    break;
                case Keys.NumPad4:
                    fourButton.PerformClick();
                    break;
                case Keys.D4:
                    fourButton.PerformClick();
                    break;
                case Keys.NumPad5:
                    fiveButton.PerformClick();
                    break;
                case Keys.D5:
                    fiveButton.PerformClick();
                    break;
                case Keys.NumPad6:
                    sixButton.PerformClick();
                    break;
                case Keys.D6:
                    sixButton.PerformClick();
                    break;
                case Keys.NumPad7:
                    severnButton.PerformClick();
                    break;
                case Keys.D7:
                    severnButton.PerformClick();
                    break;
                case Keys.NumPad8:
                    eightButton.PerformClick();
                    break;
                case Keys.D8:
                    eightButton.PerformClick();
                    break;
                case Keys.NumPad9:
                    nineButton.PerformClick();
                    break;
                case Keys.D9:
                    nineButton.PerformClick();
                    break;
                case Keys.Divide:
                    divideButton.PerformClick();
                    break;
                case Keys.Multiply:
                    myltipyButton.PerformClick();
                    break;
                case Keys.Subtract:
                    minusButton.PerformClick();
                    break;
                case Keys.Add:
                    plusButton.PerformClick();
                    break;
                default:
                    break;
            }

        }

        private void Calculator_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) 
            { 
            
                case Keys.Enter:
                    equalButton.PerformClick(); 
                    break;
                case Keys.Escape:
                    clearButton.PerformClick();
                    break;
                case Keys.Back:
                    backButton.PerformClick();
                    break;
                default : break;
            }

        }
    }
}
