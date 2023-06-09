﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static WindowsFormsApp1.connection_string;

namespace WindowsFormsApp1
{
    public partial class login : Form
    {
        string connection_string = Utils.ConnectionString;
        public login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(connection_string);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("select passWord from user2 where userName = '" + usernameField.Text + "'", con);
            MySqlDataReader reader = cmd.ExecuteReader();
            /*if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (passwordField.Text == reader.GetString(0))
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        DialogResult = DialogResult.Abort;
                        
                    }
                }
            }*/

            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    if (BCrypt.Net.BCrypt.Verify(passwordField.Text, reader.GetString(0)))
                    {
                        // Direct to main page
                        main_menu formname = new main_menu();
                        formname.Show();
                        this.Hide();
                        // Clear text boxes
                        usernameField.Text = "";
                        passwordField.Text = "";
                    }

                }
            }
            if (String.IsNullOrEmpty(passwordField.Text) && (String.IsNullOrEmpty(usernameField.Text)))
            {
                richTextBox1.Text = "Both fields are empty, please enter a valid Username & Password.";
            }
            else if (String.IsNullOrEmpty(passwordField.Text)){
                richTextBox1.Text = "Please enter a valid password in the Password field.";
            } 
            else if (String.IsNullOrEmpty(usernameField.Text)){
                richTextBox1.Text = "Please enter a valid username in the Username field.";
            }
            else{
                usernameField.Text = "";
                passwordField.Text = "";
                richTextBox1.Text = "The details you have entered are incorrect, please try again.";
            }
            reader.Close();
            cmd.Dispose();
            con.Close(); // close the connection
            Console.WriteLine("\nConnection successfully terminated.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // https://www.youtube.com/watch?v=bkzOvlqD1s4&t=100s
            // Used the above link to help create exit functionality
            // Copyright (c) DJ Oamen Youtube (TM) 2015 | Code (C#)
            const string messages =
            "Please confirm you wish to close the system";
            const string caption = "System Data Retrieval Closing";
            var results = MessageBox.Show(messages, caption,
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question);

            // No button was pressed
            if (results == DialogResult.Yes)
            {
                // Cancel closing form
                Application.Exit();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                passwordField.UseSystemPasswordChar = true;
                var checkBox = (CheckBox)sender;
                checkBox.Text = "View Password";
            } else
            {
                passwordField.UseSystemPasswordChar = false;
                var checkBox = (CheckBox)sender;
                checkBox.Text = "Hide Password";
            }
        }
    }
    }

