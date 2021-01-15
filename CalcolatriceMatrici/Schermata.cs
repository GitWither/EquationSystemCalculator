using System;
using System.Windows.Forms;

namespace CalcolatriceMatrici
{
    public partial class Schermata : Form
    {
        private const string modelloRisultato = "X={0}, Y={1}, Z={2}";
        public Schermata()
        {
            InitializeComponent();
        }

        private void Calcola(object sender, EventArgs e)
        {
            double
                a11, a12, a13,
                a21, a22, a23,
                a31, a32, a33;

            double.TryParse(this.a11.Text, out a11); double.TryParse(this.a12.Text, out a12); double.TryParse(this.a13.Text, out a13);
            double.TryParse(this.a21.Text, out a21); double.TryParse(this.a22.Text, out a22); double.TryParse(this.a23.Text, out a23);
            double.TryParse(this.a31.Text, out a31); double.TryParse(this.a32.Text, out a32); double.TryParse(this.a33.Text, out a33);

            double n1, n2, n3;

            double.TryParse(this.n1.Text, out n1); double.TryParse(this.n2.Text, out n2); double.TryParse(this.n3.Text, out n3);

            double delta = CalcolaDeterminante(
                a11, a12, a13, 
                a21, a22, a23,
                a31, a32, a33
                );

            double deltaX = CalcolaDeterminante(
                n1, a12, a13,
                n2, a22, a23,
                n3, a32, a33
                );

            double deltaY = CalcolaDeterminante(
                a11, n1, a13,
                a21, n2, a23,
                a31, n3, a33
                );

            double deltaZ = CalcolaDeterminante(
                a11, a12, n1,
                a21, a22, n2,
                a31, a32, n3
                );

            risultatoDecimale.Text = string.Format(modelloRisultato, deltaX / delta, deltaY / delta, deltaZ / delta);
            risultatoFrazionario.Text = string.Format(modelloRisultato, DecimaleAFrazione(deltaX / delta), DecimaleAFrazione(deltaY / delta), DecimaleAFrazione(deltaZ / delta));
        }

        private double CalcolaDeterminante( double a11, double a12, double a13,
                                            double a21, double a22, double a23,
                                            double a31, double a32, double a33)
        {
            return 
                (a11 * a22 * a33) + 
                (a12 * a23 * a31) + 
                (a13 * a21 * a32) - 
                (a13 * a22 * a31) - 
                (a11 * a23 * a32) - 
                (a12 * a21 * a33);
        }

        private string DecimaleAFrazione(double decimale)
        {
            //Calcolo frazione continua (https://en.wikipedia.org/wiki/Continued_fraction), algoritmo preso e adattato da https://stackoverflow.com/questions/5124743/algorithm-for-simplifying-decimal-to-fractions
            int segno = Math.Sign(decimale);

            if (segno == -1) decimale = Math.Abs(decimale);

            double erroreMassimo = segno == 0 ? 0 : 0;

            int numeratore = (int)Math.Floor(decimale);
            decimale -= numeratore;

            if (decimale < erroreMassimo)
            {
                return $"{segno * numeratore}/1";
            }

            if (1 - erroreMassimo < decimale)
            {
                return $"{segno * (numeratore + 1)}/1";
            }

            //0/1
            int numeratoreInferiore = 0;
            int denominatoreInferiore = 1;

            //1/1
            int numeratoreMaggiore = 1;
            int denominatoreMaggiore = 1;

            while (true)
            {
                int numeratoreCentrale = numeratoreInferiore + numeratoreMaggiore;
                int denominatoreCentrale = denominatoreInferiore + denominatoreMaggiore;

                if (denominatoreCentrale * (decimale + erroreMassimo) < numeratoreCentrale)
                {
                    numeratoreMaggiore = numeratoreCentrale;
                    denominatoreMaggiore = denominatoreCentrale;
                }
                else if (numeratoreCentrale < (decimale - erroreMassimo) * denominatoreCentrale)
                {
                    numeratoreInferiore = numeratoreCentrale;
                    denominatoreInferiore = denominatoreCentrale;
                }
                else
                {
                    return $"{(numeratore * denominatoreCentrale + numeratoreCentrale) * segno}/{denominatoreCentrale}";
                }
            }
        }
    }
}