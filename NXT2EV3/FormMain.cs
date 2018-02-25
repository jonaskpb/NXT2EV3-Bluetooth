using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NXT2EV3.Helpers;
using EV3 = MonoBrick.EV3;
using NXT = MonoBrick.NXT;

namespace NXT2EV3
{
    public partial class FormMain : Form
    {
        private List<BluetoothDevice> BluetoothDevices;

        private bool Loop = true;

        private Task Worker;

        public FormMain()
        {            
            InitializeComponent();

            BluetoothDevices = BluetoothHelper.GetBluetoothPort();

            BluetoothDevices.ForEach(x => selectNxtPort.Items.Add(x));
            BluetoothDevices.ForEach(x => selectEv3Port.Items.Add(x));
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            if (selectNxtPort.SelectedItem != null && selectEv3Port.SelectedItem != null)
            {
                string nxtPort = ((BluetoothDevice)selectNxtPort.SelectedItem).Port;
                string ev3Port = ((BluetoothDevice)selectEv3Port.SelectedItem).Port;

                var nxt = new NXT.Brick<NXT.Sensor, NXT.Sensor, NXT.Sensor, NXT.Sensor>(nxtPort);
                var ev3 = new EV3.Brick<EV3.Sensor, EV3.Sensor, EV3.Sensor, EV3.Sensor>(ev3Port);

                Worker = Task.Run(() => LoopMailBoxes(nxt, ev3));                
            }
        }

        private void LoopMailBoxes(NXT.Brick<NXT.Sensor, NXT.Sensor, NXT.Sensor, NXT.Sensor> nxt,
            EV3.Brick<EV3.Sensor, EV3.Sensor, EV3.Sensor, EV3.Sensor> ev3)
        {
            try
            {
                nxt.Connection.Open();
                ev3.Connection.Open();

                while (Loop)
                {
                    if (cbEV3Mb1.Checked)
                    {
                        byte[] mb1 = nxt.Mailbox.Read(NXT.Box.Box0, true);
                        if (tbEV3Mb1.Text != "")
                        {
                            ev3.Mailbox.Send(tbEV3Mb1.Text, mb1);
                        }
                    }
                    if (cbEV3Mb2.Checked)
                    {
                        byte[] mb2 = nxt.Mailbox.Read(NXT.Box.Box1, true);
                        if (tbEV3Mb2.Text != "")
                        {
                            ev3.Mailbox.Send(tbEV3Mb2.Text, mb2);
                        }
                    }
                    if (cbEV3Mb3.Checked)
                    {
                        byte[] mb3 = nxt.Mailbox.Read(NXT.Box.Box2, true);
                        if (tbEV3Mb3.Text != "")
                        {
                            ev3.Mailbox.Send(tbEV3Mb3.Text, mb3);
                        }
                    }
                    if (cbEV3Mb4.Checked)
                    {
                        byte[] mb4 = nxt.Mailbox.Read(NXT.Box.Box3, true);
                        if (tbEV3Mb4.Text != "")
                        {
                            ev3.Mailbox.Send(tbEV3Mb4.Text, mb4);
                        }
                    }
                    if (cbEV3Mb5.Checked)
                    {
                        byte[] mb5 = nxt.Mailbox.Read(NXT.Box.Box4, true);
                        if (tbEV3Mb5.Text != "")
                        {
                            ev3.Mailbox.Send(tbEV3Mb5.Text, mb5);
                        }
                    }
                    if (cbEV3Mb6.Checked)
                    {
                        byte[] mb6 = nxt.Mailbox.Read(NXT.Box.Box5, true);
                        if (tbEV3Mb6.Text != "")
                        {
                            ev3.Mailbox.Send(tbEV3Mb6.Text, mb6);
                        }
                    }
                    if (cbEV3Mb7.Checked)
                    {
                        byte[] mb7 = nxt.Mailbox.Read(NXT.Box.Box6, true);
                        if (tbEV3Mb7.Text != "")
                        {
                            ev3.Mailbox.Send(tbEV3Mb7.Text, mb7);
                        }
                    }
                    if (cbEV3Mb8.Checked)
                    {
                        byte[] mb8 = nxt.Mailbox.Read(NXT.Box.Box7, true);
                        if (tbEV3Mb8.Text != "")
                        {
                            ev3.Mailbox.Send(tbEV3Mb8.Text, mb8);
                        }
                    }
                    if (cbEV3Mb9.Checked)
                    {
                        byte[] mb9 = nxt.Mailbox.Read(NXT.Box.Box8, true);
                        if (tbEV3Mb9.Text != "")
                        {
                            ev3.Mailbox.Send(tbEV3Mb9.Text, mb9);
                        }
                    }
                    if (cbEV3Mb10.Checked)
                    {
                        byte[] mb10 = nxt.Mailbox.Read(NXT.Box.Box9, true);
                        if (tbEV3Mb10.Text != "")
                        {
                            ev3.Mailbox.Send(tbEV3Mb10.Text, mb10);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Es ist ein Fehler aufgetreten!");
            }
            finally
            {
                nxt.Connection.Close();
                ev3.Connection.Close();
            }            
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Loop = false;            
            
            //Ensure the worker is able to close the connections before killing the main thread

            if (Worker != null)
            {
                Task.WaitAll(Worker);
            }
        }
    }
}
