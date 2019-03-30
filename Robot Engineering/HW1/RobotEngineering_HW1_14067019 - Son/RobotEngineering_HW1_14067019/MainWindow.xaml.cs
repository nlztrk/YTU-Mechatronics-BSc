using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace RobotEngineering_HW1_14067019 {

   
    public partial class MainWindow : Window {
        public MainWindow() {

            InitializeComponent();
            RoboticArm.Content = Load3D(MODEL1, MODEL2, MODEL3, MODEL4, MODEL4);
            viewer_3D.Children.Add(RoboticArm);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        double joint1, joint2, joint3;
        double sensitivity = 10;
        bool is_started = false;
        double wanted_x;
        double wanted_y;
        double wanted_z;
        double loop_x, loop_y, loop_z;
        bool kinematics_end = false;
        public static int interval = 10;
        bool[] position_check = new bool[interval + 1];
        double minimum_distance = 1000;
        double[,] angles = new double[interval, 3];
        double[,] positions = new double[interval, 3];

        Model3DGroup ArmModel = new Model3DGroup();
        Model3D arm_link1 = null;
        Model3D arm_link2 = null;
        Model3D arm_link3 = null;
        Model3D positionmodel = null;
        Model3D positionmodel2 = null;

        ModelVisual3D RoboticArm = new ModelVisual3D();

        private const string MODEL1 = "R1.stl";
        private const string MODEL2 = "R2.stl";
        private const string MODEL3 = "R3.stl";
        private const string MODEL4 = "sp.stl";

        RotateTransform3D Rotation = new RotateTransform3D();
        TranslateTransform3D Translation = new TranslateTransform3D();


        void timer_Tick(object sender, EventArgs e)
        {
            if (kinematics_end==true)
            {
                if (position_check[0] == false)
                {
                    joint1 = angles[0, 0];
                    joint2 = angles[0, 1];
                    joint3 = angles[0, 2];
                    update_models();
                    position_check[0] = true;
                    position_check[1] = false;

                    for (int i = 2; i < interval; i++)
                    {
                        position_check[i] = true;
                    }
                }
                for (int i = 1; i < interval; i++)
                {
                    if (position_check[i] == false)
                    {
                        if (joint1 != angles[i, 0])
                        {
                            joint1 += (angles[i, 0] - angles[i-1, 0]) / sensitivity;
                            joint1 = Math.Round(joint1, 1);
                        }

                        if (joint2 != angles[i, 1])
                        {
                            joint2 += (angles[i, 1] - angles[i-1, 1]) / sensitivity;
                            joint2 = Math.Round(joint2, 1);
                        }

                        if (joint3 != angles[i, 2])
                        {
                            joint3 += (angles[i, 2] - angles[i-1, 2]) / sensitivity;
                            joint3 = Math.Round(joint3, 1);
                        }
                        update_models();
                        if (joint1==angles[i, 0] && joint2 == angles[i, 1] && joint3 == angles[i, 2])
                        {
                            position_check[i] = true;
                            position_check[i + 1] = false;
                            if (i == interval-1)
                            {
                                for (int q = 0; q < interval; q++)
                                    position_check[q] = false;
                            }
                        }
                    }
                }

            }
        }



        private Model3DGroup Load3D(string model1, string model2, string model3, string spherest, string spherest2) {

            try {



                viewer_3D.RotateGesture = new MouseGesture(MouseAction.LeftClick);
                ModelImporter import = new ModelImporter();
                arm_link1 = import.Load(model1);
                arm_link2 = import.Load(model2);
                arm_link3 = import.Load(model3);
                positionmodel = import.Load(spherest);
                positionmodel2 = import.Load(spherest2);

                Transform3DGroup transform_joint1 = new Transform3DGroup();
                Transform3DGroup transform_joint2 = new Transform3DGroup();
                Transform3DGroup transform_joint3 = new Transform3DGroup();
                Transform3DGroup transform_position1 = new Transform3DGroup();
                Transform3DGroup transform_position2 = new Transform3DGroup();


                Rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90), new Point3D(0, 0, 0));
                transform_position1.Children.Add(Rotation);
                transform_position2.Children.Add(Rotation);

                Translation = new TranslateTransform3D(Convert.ToDouble(coordinate_x.Text), Convert.ToDouble(coordinate_y.Text), Convert.ToDouble(coordinate_z.Text));
                transform_position1.Children.Add(Translation);

                Translation = new TranslateTransform3D(Convert.ToDouble(coordinate_end_x.Text), Convert.ToDouble(coordinate_end_y.Text), Convert.ToDouble(coordinate_end_z.Text));
                transform_position2.Children.Add(Translation);

                Rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90), new Point3D(0, 0, 0));
                transform_joint1.Children.Add(Rotation);


                Translation = new TranslateTransform3D(0, 0, 182.5);


                Rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90), new Point3D(0, 0, 0));
                transform_joint2.Children.Add(transform_joint1);
                transform_joint2.Children.Add(Translation);
                transform_joint2.Children.Add(Rotation);


                Translation = new TranslateTransform3D(0, 97.5,0);

                Rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0), new Point3D(0, 0, 0));
                transform_joint3.Children.Add(transform_joint2);
                transform_joint3.Children.Add(Translation);
                transform_joint3.Children.Add(Rotation);

                positionmodel.Transform = transform_position1;
                positionmodel2.Transform = transform_position2;
                arm_link1.Transform = transform_joint1;
                arm_link2.Transform = transform_joint2;
                arm_link3.Transform = transform_joint3;


                ArmModel.Children.Add(arm_link1);
                ArmModel.Children.Add(arm_link2);
                ArmModel.Children.Add(arm_link3);
                ArmModel.Children.Add(positionmodel);
                ArmModel.Children.Add(positionmodel2);

            } catch (Exception e) {
                MessageBox.Show("Exception Error:" + e.StackTrace);
            }
            is_started = true;
            update_models();
            return ArmModel;
        }




        private void update_models() {
            Transform3DGroup transform_joint1 = new Transform3DGroup();
            Transform3DGroup transform_joint2 = new Transform3DGroup();
            Transform3DGroup transform_joint3 = new Transform3DGroup();
            Transform3DGroup transform_position1 = new Transform3DGroup();
            Transform3DGroup transform_position2 = new Transform3DGroup();


            Translation = new TranslateTransform3D(Convert.ToDouble(coordinate_x.Text)-40, Convert.ToDouble(coordinate_y.Text)-30, Convert.ToDouble(coordinate_z.Text)-10);
            transform_position1.Children.Add(Translation);
            Translation = new TranslateTransform3D(Convert.ToDouble(coordinate_end_x.Text)-40, Convert.ToDouble(coordinate_end_y.Text)-30, Convert.ToDouble(coordinate_end_z.Text)-10);
            transform_position2.Children.Add(Translation);

            Rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90), new Point3D(0, 0, 0));
            transform_position1.Children.Add(Rotation);
            transform_position2.Children.Add(Rotation);

            Rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90), new Point3D(0, 0, 0));
            transform_joint1.Children.Add(Rotation);


            Rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), joint1), new Point3D(17.5, -30, 0));
            transform_joint1.Children.Add(Rotation);


            Translation = new TranslateTransform3D(0, 182.5,0);


            transform_joint2.Children.Add(Translation);
            Rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90), new Point3D(0, 0, 0));
            transform_joint2.Children.Add(Rotation);
            Rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), joint2), new Point3D(0, 200, 0));
            transform_joint2.Children.Add(Rotation);
            transform_joint2.Children.Add(transform_joint1);

            Translation = new TranslateTransform3D(100, 0, 0);
            transform_joint3.Children.Add(Translation);

            Rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0), new Point3D(0, 0, 0));
            transform_joint3.Children.Add(Rotation);
            Rotation = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), joint3), new Point3D(100, 17.5, 0));
            transform_joint3.Children.Add(Rotation);
            
            transform_joint3.Children.Add(transform_joint2);




            positionmodel.Transform = transform_position1;
            positionmodel2.Transform = transform_position2;
            arm_link1.Transform = transform_joint1;
            arm_link2.Transform = transform_joint2;
            arm_link3.Transform = transform_joint3;

        }



        public void Calculate_Angles(object sender, RoutedEventArgs e) 
        {
            kinematics_end = false;


            for (int interval_loop = 0; interval_loop < interval; interval_loop++ )
            {
                positions[interval_loop, 0] = Convert.ToDouble(coordinate_x.Text) + (Convert.ToDouble(coordinate_end_x.Text) - Convert.ToDouble(coordinate_x.Text)) * interval_loop / (interval-1);
                positions[interval_loop, 1] = Convert.ToDouble(coordinate_y.Text) + (Convert.ToDouble(coordinate_end_y.Text) - Convert.ToDouble(coordinate_y.Text)) * interval_loop / (interval - 1);
                positions[interval_loop, 2] = Convert.ToDouble(coordinate_z.Text) + (Convert.ToDouble(coordinate_end_z.Text) - Convert.ToDouble(coordinate_z.Text)) * interval_loop / (interval - 1);
            
                minimum_distance = 1000;

                wanted_x = positions[interval_loop, 0];
                wanted_y = positions[interval_loop, 1];
                wanted_z = positions[interval_loop, 2];

            for (int loop_joint1 = -150; loop_joint1 < 151; loop_joint1++) {
                for (int loop_joint2 =-140; loop_joint2 < 0; loop_joint2++) {
                    for (int loop_joint3 = 0; loop_joint3 < 101; loop_joint3++) {

                        loop_x = Math.Cos(loop_joint1 * Math.PI / 180) * (30 + 100 * Math.Cos(loop_joint2 * Math.PI / 180) + 80 * Math.Cos((loop_joint2 + loop_joint3) * Math.PI / 180));
                        loop_y = Math.Sin(loop_joint1 * Math.PI / 180) * (30 + 100 * Math.Cos(loop_joint2 * Math.PI / 180) + 80 * Math.Cos((loop_joint2 + loop_joint3) * Math.PI / 180));
                        loop_z = 200 + 100 * Math.Sin(loop_joint2 * Math.PI / 180) + 80 * Math.Sin((loop_joint2 + loop_joint3) * Math.PI / 180);

                        if (Math.Sqrt(Math.Pow(wanted_x - loop_x, 2) + Math.Pow(wanted_y - loop_y, 2) + Math.Pow(wanted_z - loop_z, 2)) < minimum_distance)
                        {
                            minimum_distance = (Math.Sqrt(Math.Pow(wanted_x - loop_x, 2) + Math.Pow(wanted_y - loop_y, 2) + Math.Pow(wanted_z - loop_z, 2)));
                            angles[interval_loop, 0] = loop_joint1;
                            angles[interval_loop, 1] = loop_joint2;
                            angles[interval_loop, 2] = loop_joint3;

                        }
                        }


                    }
                }
            }

            kinematics_end = true;


        }

        private void coordinate_x_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (is_started == true)
                update_models();
        }

        private void coordinate_y_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (is_started == true)
                update_models();
        }

        private void coordinate_z_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (is_started == true)
                update_models();
        }

        private void coordinate_end_x_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (is_started == true)
                update_models();
        }

        private void coordinate_end_y_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (is_started == true)
                update_models();
        }

        private void coordinate_end_z_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (is_started==true)
            update_models();
        }

        
        }

    }

