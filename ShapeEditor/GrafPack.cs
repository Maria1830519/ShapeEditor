using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Reflection;
using System.Drawing.Drawing2D;

namespace AssessmentComputerGraphics
{

    public partial class GrafPack : Form
    {
        //Create menu
        private MainMenu mainMenu;

        //Shapes available
        private bool selectSquareStatus = false;
        private bool selectTriangleStatus = false;
        private bool selectEllipseStatus = false;
        private bool selectRegularStatus = false;

       
        private bool selectMoveStatus = false;
        private bool selectRotateStatus = false;
        private bool selectDeleteStatus = false;
        private bool selectResizeStatus = false;
        private bool selectMirrorStatus = false;
        private bool selectClearStatus = false;

        private bool moving = false;

        private int selected_id = 0;
        private int clicknumber = 0;
        private Point one;
        private Point two;
        private Point three;

        //Pens for specific circumstances
        public Pen deletePen = new Pen(Color.White);
        public Pen blackPen = new Pen(Color.Black);

        public int sides = 0;
        public int degree = 0;
        public double resize = 0;
        List<Shape> shapes = new List<Shape>();
        
        //This way, when we loop through the list to find a shape, i will be the id for the shape and its points

        private Shape selectedShape = null;


        public GrafPack()
        {
            //InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;
            // The following approach uses menu items coupled with mouse clicks
            MainMenu mainMenu = new MainMenu();

            MenuItem createItem = new MenuItem();
            MenuItem squareItem = new MenuItem();
            MenuItem triangleItem = new MenuItem();
            MenuItem ellipseItem = new MenuItem();
            MenuItem regularItem = new MenuItem();

            MenuItem selectItem = new MenuItem();
            MenuItem moveItem = new MenuItem();
            MenuItem rotateItem = new MenuItem();
            MenuItem deleteItem = new MenuItem();
            MenuItem resizeItem = new MenuItem();
            MenuItem mirrorItem = new MenuItem();
            MenuItem clearItem = new MenuItem();

            createItem.Text = "&Create";
            squareItem.Text = "&Square";
            triangleItem.Text = "&Triangle";
            ellipseItem.Text = "&Ellipse";
            regularItem.Text = "&Polygon";
            selectItem.Text = "&Select";
            moveItem.Text = "&Move";
            rotateItem.Text = "&Rotate";
            resizeItem.Text = "&Resize";
            mirrorItem.Text = "&Reflection";
            deleteItem.Text = "&Delete";
            clearItem.Text = "&Clear";


            mainMenu.MenuItems.Add(createItem);
            mainMenu.MenuItems.Add(selectItem);
            mainMenu.MenuItems.Add(clearItem);
            createItem.MenuItems.Add(squareItem);
            createItem.MenuItems.Add(triangleItem);
            createItem.MenuItems.Add(ellipseItem);
            createItem.MenuItems.Add(regularItem);
            selectItem.MenuItems.Add(moveItem);
            selectItem.MenuItems.Add(rotateItem);
            selectItem.MenuItems.Add(mirrorItem);
            selectItem.MenuItems.Add(resizeItem);
            selectItem.MenuItems.Add(deleteItem);
            

            selectItem.Click += new System.EventHandler(this.selectShape);
            squareItem.Click += new System.EventHandler(this.selectSquare);
            triangleItem.Click += new System.EventHandler(this.selectTriangle);
            ellipseItem.Click += new System.EventHandler(this.selectEllipse);
            regularItem.Click += new System.EventHandler(this.selectRegular);
            moveItem.Click += new System.EventHandler(this.moveItem);
            rotateItem.Click += new System.EventHandler(this.rotateItem);
            deleteItem.Click += new System.EventHandler(this.deleteItem);
            resizeItem.Click += new System.EventHandler(this.resizeItem);
            mirrorItem.Click += new System.EventHandler(this.mirrorItem);
            clearItem.Click += new System.EventHandler(this.clearItem);
            this.Menu = mainMenu;
            this.MouseClick += mouseClick;
        }
        // Generally, all methods of the form are usually private
        private void selectSquare(object sender, EventArgs e)
        {
            selectSquareStatus = true;
            MessageBox.Show("Click OK and then click once each at two locations to create a square");
        }
        private void selectTriangle(object sender, EventArgs e)
        {
            selectTriangleStatus = true;
            MessageBox.Show("Click OK and then click once each at three locations to create a triangle");
        }
        private void selectEllipse(object sender, EventArgs e)
        {
            selectEllipseStatus = true;
            MessageBox.Show("Click OK and then click once each at two locations to create an ellipse");
        }
        private void selectRegular(object sender, EventArgs e)
        {
            bool is_number = false;
            do
            {
                //https://www.delftstack.com/howto/csharp/create-an-input-dialog-box-in-csharp/
                string input = Interaction.InputBox("Select number of sides desired for the regular polygon", "SIDES NUMBER", "Default", 250, 250);

                if (int.TryParse(input, out sides))
                {
                    sides = Int32.Parse(input);
                    is_number = true;
                }
                else
                {
                    MessageBox.Show("Please, enter a valid number");
                    is_number = false;
                }
            } while (is_number == false);         

            selectRegularStatus = true;
            MessageBox.Show("Click OK and then click once each at two locations to create a regular polygon");
        }
        private void selectShape(object sender, EventArgs e)
        {
            
            MessageBox.Show("You selected the Select option...");
        }
        private void moveItem(object sender, EventArgs e)
        {
            selectMoveStatus = true;
            MessageBox.Show("Move Selected");
        }
        private void rotateItem(object sender, EventArgs e)
        {
            
            selectRotateStatus = true;
            MessageBox.Show("Rotation Selected");
        }
        private void deleteItem(object sender, EventArgs e)
        {
            selectDeleteStatus = true;
            MessageBox.Show("Delete Selected");
        }
        private void resizeItem(object sender, EventArgs e)
        {
            selectResizeStatus = true;
            MessageBox.Show("Resize Selected");
        }
        private void mirrorItem(object sender, EventArgs e)
        {
            selectMirrorStatus = true;
            MessageBox.Show("Reflecting Selected");
        }
        private void clearItem(object sender, EventArgs e)
        {
            selectClearStatus = true;

            MessageBox.Show("Clearing the form...");
            
            Graphics g = this.CreateGraphics();
            selectClearStatus = false;
            //If clear is selected, the whole form is going to be emptied and all shapes deleted
            shapes.Clear();
            g.Clear(Color.White);

            
        }

        // This method is quite important and detects all mouse clicks - other methods may need
        // to be implemented to detect other kinds of event handling eg keyboard presses.
        private void mouseClick(object sender, MouseEventArgs e)
        {
           
            if (e.Button == MouseButtons.Left)
            {
                // 'if' statements can distinguish different selected menu operations to implement.
                // There may be other (better, more efficient) approaches to event handling,
                // but this approach works.
                if (selectSquareStatus == true)
                {
                    if (clicknumber == 0)
                    {
                        one = new Point(e.X, e.Y);
                        clicknumber = 1;
                    }
                    else
                    {
                        two = new Point(e.X, e.Y);
                        clicknumber = 0;
                        selectSquareStatus = false;
                        Graphics g = this.CreateGraphics();

                        Square aShape = new Square(one, two);
                        shapes.Add(aShape);
                        aShape.draw(g, blackPen);
                    }
                }
                else if (selectTriangleStatus == true)
                {
                    if (clicknumber == 0)
                    {
                        one = new Point(e.X, e.Y);
                        clicknumber = 1;
                    }
                    else if (clicknumber == 1)
                    {
                        two = new Point(e.X, e.Y);
                        clicknumber = 2;
                    }
                    else
                    {
                        three = new Point(e.X, e.Y);
                        clicknumber = 0;

                        selectTriangleStatus = false;
                        Graphics g = this.CreateGraphics();
                        Pen blackpen = new Pen(Color.Black);
                        Triangle aShape = new Triangle(one, two, three);
                        aShape.draw(g, blackpen);
                        shapes.Add(aShape);
                    }
                }
                else if (selectEllipseStatus == true)
                {
                    if (clicknumber == 0)
                    {
                        one = new Point(e.X, e.Y);
                        clicknumber = 1;
                    }
                    else
                    {
                        two = new Point(e.X, e.Y);
                        clicknumber = 0;
                        selectEllipseStatus = false;
                        Graphics g = this.CreateGraphics();

                        Ellipse aShape = new Ellipse(one, two);
                        shapes.Add(aShape);
                        aShape.draw(g, blackPen);
                    }
                }
                else if (selectRegularStatus == true)
                {
                    if (clicknumber == 0)
                    {
                        one = new Point(e.X, e.Y);
                        clicknumber = 1;
                    }
                    else
                    {
                        two = new Point(e.X, e.Y);
                        clicknumber = 0;
                        selectRegularStatus = false;
                        Graphics g = this.CreateGraphics();
                        
                        Regular aShape = new Regular(one, two, sides);
                        shapes.Add(aShape);
                        aShape.draw(g, blackPen);
                    }
                }
                else if (selectMoveStatus == true)
                {
                    Graphics g = this.CreateGraphics();
                    Point select_point = new Point(e.X, e.Y);
                    selectMoveStatus = false;
                    for (int i = 0; i < shapes.Count; i++)
                    {
                        if (shapes[i].containsPoint(select_point) == true)
                        {
                            Console.WriteLine("selected shape: " + i);

                            MessageBox.Show("Click where you want to  move the shape");
                            selected_id = i;
                            moving = true;

                            return;
                            //break;

                        }


                    }

                }
                else if (selectRotateStatus == true)
                {
                    Graphics g = this.CreateGraphics();
                    Point select_point = new Point(e.X, e.Y);
                    selectRotateStatus = false;
                    for (int i = 0; i < shapes.Count; i++)
                    {

                        if (shapes[i].containsPoint(select_point) == true)
                        {
                            bool is_number = false;
                            do
                            {
                                string input = Interaction.InputBox("Select degrees desired for the rotation", "ANGLES", "*insert degree*", 350, 250);

                                if (int.TryParse(input, out sides))
                                {
                                    degree = Int32.Parse(input);
                                    is_number = true;
                                }
                                else
                                {
                                    MessageBox.Show("Please, enter a valid number");
                                    is_number = false;
                                }
                            } while (is_number == false);
                            

                            

                            if (shapes[i].shapes_get() == "square")
                            {
                                Console.WriteLine("selected shape: " + i);

                                Square deleteShape = new Square(shapes[i].points_get_point1(), shapes[i].points_get_point3());

                                deleteShape.draw(g, deletePen);

                                shapes[i].rotate(g, blackPen, degree);

                                break;
                            }
                            else if (shapes[i].shapes_get() == "triangle")
                            {
                                Console.WriteLine("selected shape: " + i);

                                Triangle deleteShape = new Triangle(shapes[i].points_get_point1(), shapes[i].points_get_point2(), shapes[i].points_get_point3());
                                deleteShape.draw(g, deletePen);

                                shapes[i].rotate(g, blackPen, degree);

                                break;
                            }
                            else if (shapes[i].shapes_get() == "ellipse")
                            {
                                Console.WriteLine("selected shape: " + i);

                           
                                Ellipse deleteShape = new Ellipse(shapes[i].points_get_point1(), shapes[i].points_get_point3());
                                deleteShape.draw(g, deletePen);

                                shapes[i].rotate(g, blackPen, degree);

                                break;
                            }
                            else if (shapes[i].shapes_get() == "regular")
                            {
                                Console.WriteLine("selected shape: " + i);
                              
                                Regular deleteShape = new Regular(shapes[i].points_get_point1(), shapes[i].points_get_point3(), shapes[i].get_sides());
                                deleteShape.draw(g, deletePen);

                                shapes[i].rotate(g, blackPen,degree);

                                break;
                            }

                        }

                    }
                    Console.WriteLine("Shape selected " + selectedShape);
                }
                else if (selectDeleteStatus == true)
                {
                    Graphics g = this.CreateGraphics();
                    Point select_point = new Point(e.X, e.Y);
                    selectDeleteStatus = false;
                    for (int i = 0; i < shapes.Count; i++)
                    {
                        if (shapes[i].containsPoint(select_point) == true)
                        {
                            if (shapes[i].shapes_get() == "square")
                            {

                                Square deleteShape = new Square(shapes[i].points_get_point1(), shapes[i].points_get_point3());
                                deleteShape.draw(g, deletePen);

                                shapes.Remove(shapes[i]);

                                break;
                            }
                            else if (shapes[i].shapes_get() == "triangle")
                            {

                                Triangle deleteShape = new Triangle(shapes[i].points_get_point1(), shapes[i].points_get_point2(), shapes[i].points_get_point3());
                                deleteShape.draw(g, deletePen);

                                shapes.Remove(shapes[i]);

                                break;
                            }
                            else if (shapes[i].shapes_get() == "ellipse")
                            {

                                Ellipse deleteShape = new Ellipse(shapes[i].points_get_point1(), shapes[i].points_get_point3());
                                deleteShape.draw(g, deletePen);

                                shapes.Remove(shapes[i]);

                                break;
                            }
                            else if (shapes[i].shapes_get() == "regular")
                            { 
                                Regular deleteShape = new Regular(shapes[i].points_get_point1(), shapes[i].points_get_point3(), shapes[i].get_sides());
                                deleteShape.draw(g, deletePen);

                                shapes.Remove(shapes[i]);

                                break;
                            }

                        }

                    }

                }
                else if (selectResizeStatus == true)
                {
                    Graphics g = this.CreateGraphics();
                    Point select_point = new Point(e.X, e.Y);
                    selectResizeStatus = false;
                    for (int i = 0; i < shapes.Count; i++)
                    {
                        if (shapes[i].containsPoint(select_point) == true)
                        {
                            bool is_number = false;
                            do
                            {
                                string input = Interaction.InputBox("Introduce how much you would like to re-scale the shape", "Sie", "*insert number*", 350, 250);

                                if (double.TryParse(input, out resize))
                                {
                                    resize = Convert.ToDouble(input);
                                    is_number = true;
                                }
                                else
                                {
                                    MessageBox.Show("Please, enter a valid number");
                                    is_number = false;
                                }
                            } while (is_number == false);

                            if (shapes[i].shapes_get() == "square")
                            {
                                Console.WriteLine("selected shape: " + i);

                                Square deleteShape = new Square(shapes[i].points_get_point1(), shapes[i].points_get_point3());
                                deleteShape.draw(g, deletePen);

                                shapes[i].resize(g, blackPen, resize);

                                break;
                            }
                            else if (shapes[i].shapes_get() == "triangle")
                            {
                                Console.WriteLine("selected shape: " + i);

                                
                                Triangle deleteShape = new Triangle(shapes[i].points_get_point1(), shapes[i].points_get_point2(), shapes[i].points_get_point3());
                                deleteShape.draw(g, deletePen);

                                shapes[i].resize(g, blackPen, resize);

                                break;
                            }
                            else if (shapes[i].shapes_get() == "ellipse")
                            {
                                Console.WriteLine("selected shape: " + i);

                                
                                Ellipse deleteShape = new Ellipse(shapes[i].points_get_point1(), shapes[i].points_get_point3());
                                deleteShape.draw(g, deletePen);

                                shapes[i].resize(g, blackPen, resize);

                                break;
                            }
                            else if (shapes[i].shapes_get() == "regular")
                            {
                                Console.WriteLine("selected shape: " + i);

                               
                                Regular deleteShape = new Regular(shapes[i].points_get_point1(), shapes[i].points_get_point3(), shapes[i].get_sides());
                                deleteShape.draw(g, deletePen);

                                shapes[i].resize(g, blackPen,resize);

                                break;
                            }

                        }

                    }

                }
                else if (selectMirrorStatus == true)
                {
                    Graphics g = this.CreateGraphics();
                    Point select_point = new Point(e.X, e.Y);
                    selectMirrorStatus = false;
                    for (int i = 0; i < shapes.Count; i++)
                    {
                        if (shapes[i].containsPoint(select_point) == true)
                        {
                            if (shapes[i].shapes_get() == "square")
                            {
                                Console.WriteLine("selected shape: " + i);

                              
                                Square deleteShape = new Square(shapes[i].points_get_point1(), shapes[i].points_get_point3());
                                deleteShape.draw(g, deletePen);

                                shapes[i].mirror(g, blackPen);

                                break;
                            }
                            else if (shapes[i].shapes_get() == "triangle")
                            {
                                Console.WriteLine("selected shape: " + i);

                                Triangle deleteShape = new Triangle(shapes[i].points_get_point1(), shapes[i].points_get_point2(), shapes[i].points_get_point3());
                                deleteShape.draw(g, deletePen);

                                shapes[i].mirror(g, blackPen);

                                break;
                            }
                            else if (shapes[i].shapes_get() == "ellipse")
                            {
                                MessageBox.Show("Unfortunately, an Ellipse cannot be reflected");
                            }
                            else if (shapes[i].shapes_get() == "regular")
                            {
                                MessageBox.Show("Unfortunately, an Regular polygon cannot be reflected");
                            }

                        }

                    }

                }
                
            }
            //This is separated to the other so that the click to select the shape is different to the one where the shape is being moved
            if (moving)
            {
                Graphics g = this.CreateGraphics();
                if (shapes[selected_id].shapes_get() == "square")
                {
                    Point move_place = new Point(e.X, e.Y);

                    Square deleteShape = new Square(shapes[selected_id].points_get_point1(), shapes[selected_id].points_get_point3());
                    deleteShape.draw(g, deletePen);

                    shapes[selected_id].move(g, blackPen, move_place);


                    moving = false;
                    return;

                }
                else if (shapes[selected_id].shapes_get() == "triangle")
                {
                    Point move_place = new Point(e.X, e.Y);

                    Triangle deleteShape = new Triangle(shapes[selected_id].points_get_point1(), shapes[selected_id].points_get_point2(), shapes[selected_id].points_get_point3());
                    deleteShape.draw(g, deletePen);

                    shapes[selected_id].move(g, blackPen, move_place);

                    moving = false;
                    return;
                }
                else if (shapes[selected_id].shapes_get() == "ellipse")
                {
                    Point move_place = new Point(e.X, e.Y);

                    Ellipse deleteShape = new Ellipse(shapes[selected_id].points_get_point1(), shapes[selected_id].points_get_point3());
                    deleteShape.draw(g, deletePen);

                    shapes[selected_id].move(g, blackPen, move_place);

                    moving = false;
                    return;
                }
                else if (shapes[selected_id].shapes_get() == "regular")
                {
                    Point move_place = new Point(e.X, e.Y);

                    Regular deleteShape = new Regular(shapes[selected_id].points_get_point1(), shapes[selected_id].points_get_point3(), sides);
                    deleteShape.draw(g, deletePen);

                    shapes[selected_id].move(g, blackPen, move_place);
                    moving = false;
                    return;
                }



            }

        }
    }

    abstract class Shape
    {

        // This is the base class for Shapes in the application. It should allow an array or LL
        // to be created containing different kinds of shapes.
        public Shape() //Constructor
        {

        }
        public abstract void draw(Graphics g, Pen pen);
        public abstract bool containsPoint(Point p);
        public abstract void rotate(Graphics g, Pen blackPen, int degree);
        public abstract void points_set(Point x, Point y, Point z);
        public abstract Point points_get_point1();
        public abstract Point points_get_point2();
        public abstract Point points_get_point3();
        public abstract String shapes_get();
        public abstract int get_sides();
        public abstract int set_sides(int s);
        public abstract void move(Graphics g, Pen blackPen, Point move);
        public abstract void resize (Graphics g, Pen blackPen, double resize);
        public abstract void mirror(Graphics g, Pen blackPen);

    }
    class Square : Shape
    {
        //This class contains the specific details for a square defined in terms of opposite corners
        Point point1, point3; // these points identify opposite corners of the square
        
        public Square(Point point1, Point point3) // constructor
        {
            this.point1 = point1;
            this.point3 = point3;
            
        }

        public override void draw(Graphics g, Pen blackPen)
        {
            // This method draws the square by calculating the positions of the other 2 corners
            double xDiff, yDiff, xMid, yMid; // range and mid points of x & y
                                             // calculate ranges and mid points
            xDiff = point3.X - point1.X;
            yDiff = point3.Y - point1.Y;
            xMid = (point3.X + point1.X) / 2;
            yMid = (point3.Y + point1.Y) / 2;
            ;
            // draw square
            g.DrawLine(blackPen, (int)point1.X, (int)point1.Y, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
            g.DrawLine(blackPen, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2), (int)point3.X, (int)point3.Y);
            g.DrawLine(blackPen, (int)point3.X, (int)point3.Y, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));
            g.DrawLine(blackPen, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2), (int)point1.X, (int)point1.Y);

        }
        public override bool containsPoint(Point p)
        {
            //Only click inside the shape
            int minX, minY, maxX, maxY;
            minX = Math.Min(point1.X, point3.X);
            minY = Math.Min(point1.Y, point3.Y);
            maxX = Math.Max(point1.X, point3.X);
            maxY = Math.Max(point1.Y, point3.Y);

            //Find the points of the external square
            Point minPoint = new Point(minX, minY);
            Point maxPoint = new Point(maxX, maxY);

            if (p.X >= minX && p.X <= maxX && p.Y >= minY && p.Y <= maxY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void rotate(Graphics g, Pen testPen, int degree)
        {
            // This method draws the square by calculating the positions of the other 2 corners
            double xDiff, yDiff, xMid, yMid;   // range and mid points of x & y  
            Point rotated1 = new Point();
            Point rotated2 = new Point();
            Point rotated3 = new Point();
            Point rotated4 = new Point();

            // calculate ranges and mid points
            xDiff = point3.X - point1.X;
            yDiff = point3.Y - point1.Y;
            xMid = (point3.X + point1.X) / 2;
            yMid = (point3.Y + point1.Y) / 2;


            //Calculate missing points
            Point point2 = new Point((int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
            Point point4 = new Point((int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));

            //Calculate axis for rotation
            Point axis = centre(point1, point3);

            //"Move" the shape to coordinates as if centre would be (0,0), otherwise the
            //shape will rotate not only over its centre but also over coordinate (0,0) of the form
            //which means the shape would be moved
            Point transformed1 = new Point((int)point1.X - axis.X, (int)point1.Y - axis.Y);
            Point transformed2 = new Point((int)point2.X - axis.X, (int)point2.Y - axis.Y);
            Point transformed3 = new Point((int)point3.X - axis.X, (int)point3.Y - axis.Y);
            Point transformed4 = new Point((int)point4.X - axis.X, (int)point4.Y - axis.Y);

            //Rotate transfromed points
            double angle = degree;
            rotated1.X = (int)(transformed1.X * Math.Cos(angle * Math.PI / 180) - transformed1.Y * Math.Sin(angle * Math.PI / 180));
            rotated1.Y = (int)(transformed1.X * Math.Sin(angle * Math.PI / 180) + transformed1.Y * Math.Cos(angle * Math.PI / 180));
            rotated3.X = (int)(transformed3.X * Math.Cos(angle * Math.PI / 180) - transformed3.Y * Math.Sin(angle * Math.PI / 180));
            rotated3.Y = (int)(transformed3.X * Math.Sin(angle * Math.PI / 180) + transformed3.Y * Math.Cos(angle * Math.PI / 180));
            rotated2.X = (int)(transformed2.X * Math.Cos(angle * Math.PI / 180) - transformed2.Y * Math.Sin(angle * Math.PI / 180));
            rotated2.Y = (int)(transformed2.X * Math.Sin(angle * Math.PI / 180) + transformed2.Y * Math.Cos(angle * Math.PI / 180));
            rotated4.X = (int)(transformed4.X * Math.Cos(angle * Math.PI / 180) - transformed4.Y * Math.Sin(angle * Math.PI / 180));
            rotated4.Y = (int)(transformed4.X * Math.Sin(angle * Math.PI / 180) + transformed4.Y * Math.Cos(angle * Math.PI / 180));

            //Revert the previous movement to coordenates (0,0) to maintain initial centre point
            transformed1 = new Point((int)rotated1.X + axis.X, (int)rotated1.Y + axis.Y);
            transformed2 = new Point((int)rotated2.X + axis.X, (int)rotated2.Y + axis.Y);
            transformed3 = new Point((int)rotated3.X + axis.X, (int)rotated3.Y + axis.Y);
            transformed4 = new Point((int)rotated4.X + axis.X, (int)rotated4.Y + axis.Y);

            //Reassign the original values point1 and point3 with the new transformed values :)
            this.point1 = transformed1;
            this.point3 = transformed3;

            //rotated square
            g.DrawLine(testPen, (int)transformed1.X, (int)transformed1.Y, transformed2.X, transformed2.Y);
            g.DrawLine(testPen, transformed2.X, transformed2.Y, (int)transformed3.X, (int)transformed3.Y);
            g.DrawLine(testPen, (int)transformed3.X, (int)transformed3.Y, transformed4.X, transformed4.Y);
            g.DrawLine(testPen, transformed4.X, transformed4.Y, (int)transformed1.X, (int)transformed1.Y);

        }
        public override void points_set(Point x, Point y, Point z)
        {
            point1 = x;
            point3 = y;
        }
        public Point centre(Point a, Point b)
        {
            //Calculate centre
            double xMid = (point3.X + point1.X) / 2;
            double yMid = (point3.Y + point1.Y) / 2;

            Point axis = new Point((int)xMid, (int)yMid);
            return axis;

        }
        public override Point points_get_point1()
        {
            return point1;

        }
        public override Point points_get_point2()
        {
            //This method is only useful for triangle, other shapes will not access it
            Point point2 = new Point(0, 0);
            return point2;

        }
        public override Point points_get_point3()
        {
            return point3;

        }
        public override String shapes_get()
        {
            return "square";
        }
        public override void move(Graphics g, Pen testPen, Point move)
        {
            //sum new point to centre
            //to point 1 we substract x  and y
            int dx = move.X - centre(point1, point3).X;
            int dy = move.Y - centre(point1, point3).Y;

            Point new_point1 = new Point(point1.X + dx, point1.Y + dy);
            Point new_point3 = new Point(point3.X + dx, point3.Y + dy);

            this.point1 = new_point1;
            this.point3 = new_point3;

            double xDiff, yDiff, xMid, yMid; // range and mid points of x & y

           // calculate ranges and mid points
            xDiff = point3.X - point1.X;
            yDiff = point3.Y - point1.Y;
            xMid = (point3.X + point1.X) / 2;
            yMid = (point3.Y + point1.Y) / 2;

            // draw square
            g.DrawLine(testPen, (int)point1.X, (int)point1.Y, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
            g.DrawLine(testPen, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2), (int)point3.X, (int)point3.Y);
            g.DrawLine(testPen, (int)point3.X, (int)point3.Y, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));
            g.DrawLine(testPen, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2), (int)point1.X, (int)point1.Y);
        }

        /// <summary>
        /// //This is only useful for regular polygon, please dont use
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public override int set_sides(int s)
        {
            int sides = s;
            return sides;
        }
        public override int get_sides()
        {
            int sides = 0;
            return sides;
        }
        public override void resize(Graphics g, Pen blackPen, double resize)
        {
            
            //Scale square points regarding its centre:
            point1.X = centre(point1, point3).X + (point1.X - centre(point1, point3).X) * (int)resize;
            point1.Y = centre(point1, point3).Y + (point1.Y - centre(point1, point3).Y) * (int)resize;
            point3.X = centre(point1, point3).X + (point3.X - centre(point3, point3).X) * (int)resize;
            point3.Y = centre(point1, point3).Y + (point3.Y - centre(point3, point3).Y) * (int)resize;

            double xDiff, yDiff, xMid, yMid; // range and mid points of x & y
                                             // calculate ranges and mid points
            xDiff = point3.X - point1.X;
            yDiff = point3.Y - point1.Y;
            xMid = (point3.X + point1.X) / 2;
            yMid = (point3.Y + point1.Y) / 2;

            // draw square
            g.DrawLine(blackPen, (int)point1.X, (int)point1.Y, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
            g.DrawLine(blackPen, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2), (int)point3.X, (int)point3.Y);
            g.DrawLine(blackPen, (int)point3.X, (int)point3.Y, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));
            g.DrawLine(blackPen, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2), (int)point1.X, (int)point1.Y);
        }

        public override void mirror(Graphics g, Pen blackPen)
        {

            //For mirroring x' = -x & y' = y
            //The mirroring normally works on a grid where the mirror axis is done on 0,0
            //The goal is to adapt this axis mirror to use the centre point of the shape instead of 0,0
            Point axis = new Point(centre(point1, point3).X, centre(point1, point3).Y);

            Point mirror1 = new Point(axis.X - (point1.X-axis.X), point1.Y);
            Point mirror3 = new Point(axis.X - (point3.X- axis.X), point3.Y);

            this.point1 = mirror1;
            this.point3 = mirror3;


            double xDiff, yDiff, xMid, yMid; // range and mid points of x & y
                                             // calculate ranges and mid points
            xDiff = point3.X - point1.X;
            yDiff = point3.Y - point1.Y;
            xMid = (point3.X + point1.X) / 2;
            yMid = (point3.Y + point1.Y) / 2;

            // draw square
            g.DrawLine(blackPen, (int)point1.X, (int)point1.Y, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
            g.DrawLine(blackPen, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2), (int)point3.X, (int)point3.Y);
            g.DrawLine(blackPen, (int)point3.X, (int)point3.Y, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));
            g.DrawLine(blackPen, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2), (int)point1.X, (int)point1.Y);
        }

    }
    class Triangle : Shape
    {
        //This class contains the specific details for a triangle
        Point point1, point2, point3; // these points identify the three vertex of a triangle
        List<Point> points = new List<Point>();
        public Triangle(Point point1, Point point2, Point point3) // constructor
        {
            this.point1 = point1;
            this.point2 = point2;
            this.point3 = point3;
        }


        public override void draw(Graphics g, Pen blackPen)
        {
            // This method draws the Triangle by linking the three points
            // draw triangle
            g.DrawLine(blackPen, (int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
            g.DrawLine(blackPen, (int)point2.X, (int)point2.Y, (int)point3.X, (int)point3.Y);
            g.DrawLine(blackPen, (int)point3.X, (int)point3.Y, (int)point1.X, (int)point1.Y);
        }
        public override bool containsPoint(Point p)
        {
            //Only click inside the shape
            int minX, minY, maxX, maxY;
            minX = Math.Min(point1.X, point3.X);
            minY = Math.Min(point1.Y, point3.Y);
            maxX = Math.Max(point1.X, point3.X);
            maxY = Math.Max(point1.Y, point3.Y);

            minX = Math.Min(minX, point2.X);
            minY = Math.Min(minY, point2.Y);
            maxX = Math.Max(maxX, point2.X);
            maxY = Math.Max(maxY, point2.Y);

            //Find the points of the external triangle
            Point minPoint = new Point(minX, minY);
            Point maxPoint = new Point(maxX, maxY);

            if (p.X >= minX && p.X <= maxX && p.Y >= minY && p.Y <= maxY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void rotate(Graphics g, Pen testPen, int degree)
        {

            Point rotated1 = new Point();
            Point rotated2 = new Point();
            Point rotated3 = new Point();

            //steps to calculate the centre of a triangle
            int x = (point1.X + point2.X + point3.X) / 3;
            int y = (point1.Y + point2.Y + point3.Y) / 3;

            Point axis = new Point(x, y);

            Point transformed1 = new Point((int)point1.X - axis.X, (int)point1.Y - axis.Y);
            Point transformed2 = new Point((int)point2.X - axis.X, (int)point2.Y - axis.Y);
            Point transformed3 = new Point((int)point3.X - axis.X, (int)point3.Y - axis.Y);

            //Rotate transformed points
            double angle = degree;
            rotated1.X = (int)(transformed1.X * Math.Cos(angle * Math.PI / 180) - transformed1.Y * Math.Sin(angle * Math.PI / 180));
            rotated1.Y = (int)(transformed1.X * Math.Sin(angle * Math.PI / 180) + transformed1.Y * Math.Cos(angle * Math.PI / 180));
            rotated3.X = (int)(transformed3.X * Math.Cos(angle * Math.PI / 180) - transformed3.Y * Math.Sin(angle * Math.PI / 180));
            rotated3.Y = (int)(transformed3.X * Math.Sin(angle * Math.PI / 180) + transformed3.Y * Math.Cos(angle * Math.PI / 180));
            rotated2.X = (int)(transformed2.X * Math.Cos(angle * Math.PI / 180) - transformed2.Y * Math.Sin(angle * Math.PI / 180));
            rotated2.Y = (int)(transformed2.X * Math.Sin(angle * Math.PI / 180) + transformed2.Y * Math.Cos(angle * Math.PI / 180));
            

            //Revert the initial transfromation
            transformed1 = new Point((int)rotated1.X + axis.X, (int)rotated1.Y + axis.Y);
            transformed2 = new Point((int)rotated2.X + axis.X, (int)rotated2.Y + axis.Y);
            transformed3 = new Point((int)rotated3.X + axis.X, (int)rotated3.Y + axis.Y);
            

            //Reassign the original values point1, point2 and point3 with the new transformed values :)
            this.point1 = transformed1;
            this.point1 = transformed2;
            this.point3 = transformed3;

            //rotated triangle
            g.DrawLine(testPen, transformed1.X, transformed1.Y, transformed2.X, transformed2.Y);
            g.DrawLine(testPen, transformed2.X, transformed2.Y, transformed3.X, transformed3.Y);
            g.DrawLine(testPen, transformed3.X, transformed3.Y, transformed1.X, transformed1.Y);

        }
        public override void points_set(Point x, Point y, Point z)
        {
            point1 = x;
            point2 = y;
            point3 = z;
        }
        public Point centre(Point a, Point b, Point c)
        {

            double xMid = (point3.X + point1.X) / 2;
            double yMid = (point3.Y + point1.Y) / 2;

            Point axis = new Point((int)xMid, (int)yMid);
            return axis;

        }
        public override Point points_get_point1()
        {
            return point1;

        }
        public override Point points_get_point2()
        {
            return point2;

        }
        public override Point points_get_point3()
        {
            return point3;

        }
        public override String shapes_get()
        {
            return "triangle";
        }
        public override void move(Graphics g, Pen blackPen, Point move)
        {
            //sum new point to centre
            //to point 1 we substract x  and y
            int dx = move.X - centre(point1, point2, point3).X;
            int dy = move.Y - centre(point1, point2, point3).Y;

            Point new_point1 = new Point(point1.X + dx, point1.Y + dy);
            Point new_point2 = new Point(point2.X + dx, point2.Y + dy);
            Point new_point3 = new Point(point3.X + dx, point3.Y + dy);

            this.point1 = new_point1;
            this.point2 = new_point2;
            this.point3 = new_point3;


            // draw square
            g.DrawLine(blackPen, (int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
            g.DrawLine(blackPen, (int)point2.X, (int)point2.Y, (int)point3.X, (int)point3.Y);
            g.DrawLine(blackPen, (int)point3.X, (int)point3.Y, (int)point1.X, (int)point1.Y);


        }
        //This is only useful for regular polygon, please dont use

        public override int set_sides(int s)
        {
            int sides = s;
            return sides;
        }
        public override int get_sides()
        {
            int sides = 0;
            return sides;
        }
        public override void resize(Graphics g, Pen blackPen, double resize)
        {
            //Calculates the new position of each coordenate to resize over the centre of the original shape
            point1.X = centre(point1,point2, point3).X + (point1.X - centre(point1, point2, point3).X) * (int)resize;
            point1.Y = centre(point1, point2, point3).Y + (point1.Y - centre(point1, point2, point3).Y) * (int)resize;
            point2.X = centre(point2, point2, point3).X + (point2.X - centre(point1, point2, point3).X) * (int)resize;
            point2.Y = centre(point2, point2, point3).Y + (point2.Y - centre(point1, point2, point3).Y) * (int)resize;
            point3.X = centre(point1, point2, point3).X + (point3.X - centre(point3, point2, point3).X) * (int)resize;
            point3.Y = centre(point1, point2, point3).Y + (point3.Y - centre(point3, point2, point3).Y) * (int)resize;

            //Draw new triangle
            g.DrawLine(blackPen, (int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
            g.DrawLine(blackPen, (int)point2.X, (int)point2.Y, (int)point3.X, (int)point3.Y);
            g.DrawLine(blackPen, (int)point3.X, (int)point3.Y, (int)point1.X, (int)point1.Y);
        }
        public override void mirror(Graphics g, Pen blackPen)
        {
            //For mirroring x' = -x & y' = y
            //The mirroring normally works on a grid where the mirror axis is done on 0,0
            //The goal is to adapt this axis mirror to use the centre point of the shape instead of 0,0
            Point axis = new Point(centre(point1, point2, point3).X, centre(point1, point2, point3).Y);


            Point mirror1 = new Point(axis.X - (point1.X - axis.X), point1.Y);
            Point mirror2 = new Point(axis.X - (point2.X - axis.X), point2.Y);
            Point mirror3 = new Point(axis.X - (point3.X - axis.X), point3.Y);

            this.point1 = mirror1;
            this.point2 = mirror2;
            this.point3 = mirror3;



            // draw triangle
            g.DrawLine(blackPen, (int)point1.X, (int)point1.Y, (int)point2.X, (int)point2.Y);
            g.DrawLine(blackPen, (int)point2.X, (int)point2.Y, (int)point3.X, (int)point3.Y);
            g.DrawLine(blackPen, (int)point3.X, (int)point3.Y, (int)point1.X, (int)point1.Y);
        }
    }
    class Ellipse : Shape
    {
        //This class contains the specific details for an ellipse
        Point point1, point3; 
        List<Point> points = new List<Point>();
        public Ellipse(Point point1, Point point3) // constructor
        {
            this.point1 = point1;
            this.point3 = point3;
        }

        public override void draw(Graphics g, Pen blackPen)
        {
            // This method draws the Ellipse by getting the opposite points of an imaginar rectangle

            float x = point1.X;
            float y = point1.Y;
            float width = point3.X - point1.X;
            float height = (point3.Y - point1.Y);



            g.DrawEllipse(blackPen, x, y, width, height);
        }
        public override bool containsPoint(Point p)
        {
            //Only click inside the shape
            int minX, minY, maxX, maxY;
            minX = Math.Min(point1.X, point3.X);
            minY = Math.Min(point1.Y, point3.Y);
            maxX = Math.Max(point1.X, point3.X);
            maxY = Math.Max(point1.Y, point3.Y);

            //Find the points of the external ellipse
            Point minPoint = new Point(minX, minY);
            Point maxPoint = new Point(maxX, maxY);

            if (p.X >= minX && p.X <= maxX && p.Y >= minY && p.Y <= maxY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void rotate(Graphics g, Pen blackPen, int degree)
        {
            float x = point1.X;
            float y = point1.Y;
            float width = point3.X - point1.X;
            float height = point3.Y - point1.Y;

            //Unfortunate, this could not be done with cos and sin as it would, for an unknown reason, change the size of the shape
            // Calculate the centre point of the ellipse
            PointF centre = new PointF(x + width / 2, y + height / 2);

            // Save the current transformation
            Matrix matrix = g.Transform;

            // Translate to the centre point of the ellipse
            g.TranslateTransform(centre.X, centre.Y);

            // Rotate the graphics around the centre point of the ellipse
            g.RotateTransform(degree);

            // Translate back to the original position
            g.TranslateTransform(-centre.X, -centre.Y);

            // Draw the ellipse
            g.DrawEllipse(blackPen, x, y, width, height);

            // Restore the previous transformation

           
            g.Transform = matrix;
        }
        

        public override void points_set(Point x, Point y, Point z)
        {
            point1 = x;
            point3 = z;
        }
        public Point centre(Point a, Point c)
        {

            double xMid = (point3.X + point1.X) / 2;
            double yMid = (point3.Y + point1.Y) / 2;

            Point axis = new Point((int)xMid, (int)yMid);
            return axis;

        }
        public override Point points_get_point1()
        {
            return point1;

        }
        public override Point points_get_point2()
        {
            return point1;

        }

        public override Point points_get_point3()
        {
            return point3;

        }
        public override String shapes_get()
        {
            return "ellipse";
        }
        public override void move(Graphics g, Pen blackPen, Point move)
        {
            //sum new point to centre
            //to point 1 we substract x  and y
            int dx = move.X - centre(point1, point3).X;
            int dy = move.Y - centre(point1, point3).Y;

            Point new_point1 = new Point(point1.X + dx, point1.Y + dy);
            Point new_point3 = new Point(point3.X + dx, point3.Y + dy);

            this.point1 = new_point1;
            this.point3 = new_point3;

            float x = point1.X;
            float y = point1.Y;
            float width = point3.X - point1.X;
            float height = (point3.Y - point1.Y);

            g.DrawEllipse(blackPen, x, y, width, height);
        }


        public override int set_sides(int s)
        {
            int sides = s;
            return sides;
        }
        public override int get_sides()
        {
            int sides = 0;
            return sides;
        }
        public override void resize(Graphics g, Pen blackPen, double resize)
        {

            //Calculates the new position of each coordenate to resize over the centre of the original shape
            point1.X = centre(point1, point3).X + (point1.X - centre(point1, point3).X) * (int)resize;
            point1.Y = centre(point1, point3).Y + (point1.Y - centre(point1, point3).Y) * (int)resize;
            point3.X = centre(point1, point3).X + (point3.X - centre(point3, point3).X) * (int)resize;
            point3.Y = centre(point1, point3).Y + (point3.Y - centre(point3, point3).Y) * (int)resize;


            float x = point1.X;
            float y = point1.Y;
            float width = point3.X - point1.X;
            float height = (point3.Y - point1.Y);



            g.DrawEllipse(blackPen, x, y, width, height);
        }
        public override void mirror(Graphics g, Pen blackPen)
        {
            //Unfortunately, this ellipse cannot be mirrored

        }
    }
    class Regular : Shape
    {
        //This class contains the specific details for a regular polygon defined in terms of opposite corners
        Point point1, point3; // these points identify the start and end of the diameter of the shape
        int sides;

        List<Point> points = new List<Point>();
        public Regular(Point point1, Point point3, int sides) // constructor
        {
            this.point1 = point1;
            this.point3 = point3;
            this.sides = sides;

            //this.points = new List<Point>() { point1, point3 };
        }

        public override void draw(Graphics g, Pen blackPen)
        {
            Console.WriteLine("P1: " + point1.X + " - " + point1.Y);
            Console.WriteLine("P3: " + point3.X + " - " + point3.Y);

            // This method draws the Polygon by choosing point1 as first vertex and point3 as centre.
            //r is radius
            //The central angle between two consecutive vertices is 2π/n
            //double radius = (Point1)
            double radius = (Math.Sqrt(Math.Pow(point1.X - point3.X,2) + Math.Pow(point1.Y - point3.Y, 2)))/2;

            //We use radius to calculate the length of each of the sides
            int length = (int)(radius * 2 * Math.Sin(Math.PI / sides));

            //Caclulate new centre point
            
            //Copy of the first point
            Point temp_point1 = new Point(point1.X, point1.Y);
            //A second temporal copy that we will manipulate at the beginning of every loop
            Point temp_point2 = new Point(point1.X, point1.Y);
            for (int i = 0; i <= sides; i++)
            {
                double angle = 2 * Math.PI * (i + 1) / sides;
                temp_point2 = new Point((int)(centre(point1,point3).X + length * Math.Cos(angle)), (int)(centre(point1, point3).Y + length * Math.Sin(angle)));
                g.DrawLine(blackPen, temp_point1.X, temp_point1.Y, temp_point2.X, temp_point2.Y);
                temp_point1 = new Point(temp_point2.X, temp_point2.Y);
            }
            Pen deletePen = new Pen(Color.White);
            g.DrawLine(deletePen, point1.X, point1.Y, (int)(centre(point1,point3).X + length * Math.Cos(2 * Math.PI * 1 / sides)),  (int)(centre(point1,point3).Y + length * Math.Sin(2 * Math.PI * 1 / sides)));
            
        }
        public override bool containsPoint(Point p)
        {
            //Only click inside the shape
            int minX, minY, maxX, maxY;
            minX = Math.Min(point1.X, point3.X);
            minY = Math.Min(point1.Y, point3.Y);
            maxX = Math.Max(point1.X, point3.X);
            maxY = Math.Max(point1.Y, point3.Y);

            //Find the points of the external square
            Point minPoint = new Point(minX, minY);
            Point maxPoint = new Point(maxX, maxY);

            if (p.X >= minX && p.X <= maxX && p.Y >= minY && p.Y <= maxY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void rotate(Graphics g, Pen blackPen, int degree)
        { 
        
            
            // This method draws the regular polygon by calculating the positions of the other 2 corners
              // range and mid points of x & y  
            Point rotated1 = new Point();
            Point rotated3 = new Point();

            //Calculate the ccentre axis
            Point axis = centre(point1, point3);

            Point transformed1 = new Point((int)point1.X - axis.X, (int)point1.Y - axis.Y);
            Point transformed3 = new Point((int)point3.X - axis.X, (int)point3.Y - axis.Y);

            //Rotate transformed points
            
            rotated1.X = (int)(transformed1.X * Math.Cos(degree * Math.PI / 180) - transformed1.Y * Math.Sin(degree * Math.PI / 180));
            rotated1.Y = (int)(transformed1.X * Math.Sin(degree * Math.PI / 180) + transformed1.Y * Math.Cos(degree * Math.PI / 180));
            rotated3.X = (int)(transformed3.X * Math.Cos(degree * Math.PI / 180) - transformed3.Y * Math.Sin(degree * Math.PI / 180));
            rotated3.Y = (int)(transformed3.X * Math.Sin(degree * Math.PI / 180) + transformed3.Y * Math.Cos(degree * Math.PI / 180));

            //Revert initial transfromation
            transformed1 = new Point((int)rotated1.X + axis.X, (int)rotated1.Y + axis.Y);
            transformed3 = new Point((int)rotated3.X + axis.X, (int)rotated3.Y + axis.Y);

            this.point1 = transformed1;
            this.point3 = transformed3;
            

            // This method draws the Polygon by choosing point1 as first vertex and point3 as centre.
            //r is radius
            //The central angle between two consecutive vertices is 2π/n
            //double radius = (Point1)
            double radius = (Math.Sqrt(Math.Pow(point1.X - point3.X, 2) + Math.Pow(point1.Y - point3.Y, 2))) / 2;

            //We use radius to calculate the length of each of the sides
            int length = (int)(radius * 2 * Math.Sin(Math.PI / sides));


            //Copy of the first point
            Point temp_point1 = new Point(point1.X, point1.Y);
            //A second temporal copy that we will manipulate at the beginning of every loop
            Point temp_point2 = new Point(point1.X, point1.Y);

            double xMid = (point3.X + point1.X) / 2;
            double yMid = (point3.Y + point1.Y) / 2;
            Pen testpen = new Pen(Color.BlueViolet);
            Pen deletePen = new Pen(Color.White);
            //We need to create an additional centre as there is already a centre variable that focuses  on rotating the item from its centre point
            Point eje2 = new Point((int)xMid, (int)yMid);
            for (int i = 0; i <= sides; i++)
            {
                double internal_angle = 2 * Math.PI * (i + 1) / sides;
                temp_point2 = new Point((int)(eje2.X + length * Math.Cos(degree + internal_angle)), (int)(eje2.Y + length * Math.Sin(degree + internal_angle)));
                if (i == 0)
                {
                    g.DrawLine(deletePen, temp_point1.X, temp_point1.Y, temp_point2.X, temp_point2.Y);
                }
                else
                {
                    g.DrawLine(blackPen, temp_point1.X, temp_point1.Y, temp_point2.X, temp_point2.Y);
                }
                
                temp_point1 = new Point(temp_point2.X, temp_point2.Y);
            }
           

        }

        public override void points_set(Point x, Point y, Point z)
        {
            point1 = x;
            point3 = z;

        }
        public Point centre(Point a, Point c)
        {

            double xMid = (point3.X + point1.X) / 2;
            double yMid = (point3.Y + point1.Y) / 2;

            Point axis = new Point((int)xMid, (int)yMid);
            return axis;

        }
        public override Point points_get_point1()
        {
            return point1;

        }
        public override Point points_get_point2()
        {
            Point point = new Point(0, 0);
            return point;

        }

        public override Point points_get_point3()
        {
            return point3;

        }
        public override int set_sides(int s)
        {
            this.sides = s;
            return sides;
        }

        public override int get_sides()
        {
            return sides;
        }
        public override String shapes_get()
        {
            return "regular";
        }
        public override void move(Graphics g, Pen blackPen, Point move)
        {
            //sum new point to centre
            //to point 1 we substract x  and y
            int dx = move.X - centre(point1, point3).X;
            int dy = move.Y - centre(point1, point3).Y;

            Point new_point1 = new Point(point1.X + dx, point1.Y + dy);

            Point new_point3 = new Point(point3.X + dx, point3.Y + dy);

            this.point1 = new_point1;
            this.point3 = new_point3;

            double radius = (Math.Sqrt(Math.Pow(point1.X - point3.X, 2) + Math.Pow(point1.Y - point3.Y, 2))) / 2;

            //We use radius to calculate the length of each of the sides
            int length = (int)(radius * 2 * Math.Sin(Math.PI / sides));

            //Caclulate new centre point

            //Copy of the first point
            Point temp_point1 = new Point(point1.X, point1.Y);
            //A second temporal copy that we will manipulate at the beginning of every loop
            Point temp_point2 = new Point(point1.X, point1.Y);
            for (int i = 0; i <= sides; i++)
            {
                double angle = 2 * Math.PI * (i + 1) / sides;
                temp_point2 = new Point((int)(centre(point1, point3).X + length * Math.Cos(angle)), (int)(centre(point1, point3).Y + length * Math.Sin(angle)));
                g.DrawLine(blackPen, temp_point1.X, temp_point1.Y, temp_point2.X, temp_point2.Y);
                temp_point1 = new Point(temp_point2.X, temp_point2.Y);
            }
            Pen deletePen = new Pen(Color.White);
            g.DrawLine(deletePen, point1.X, point1.Y, (int)(centre(point1, point3).X + length * Math.Cos(2 * Math.PI * 1 / sides)), (int)(centre(point1, point3).Y + length * Math.Sin(2 * Math.PI * 1 / sides)));

        }
        public override void resize(Graphics g, Pen blackPen, double resize)
        {
            //Calculates the new position of each coordenate to resize over the centre of the original shape
            point1.X = centre(point1, point3).X + (point1.X - centre(point1, point3).X) * (int)resize;
            point1.Y = centre(point1, point3).Y + (point1.Y - centre(point1, point3).Y) * (int)resize;
            point3.X = centre(point1, point3).X + (point3.X - centre(point3, point3).X) * (int)resize;
            point3.Y = centre(point1, point3).Y + (point3.Y - centre(point3, point3).Y) * (int)resize;


            double radius = (Math.Sqrt(Math.Pow(point1.X - point3.X, 2) + Math.Pow(point1.Y - point3.Y, 2))) / 2;

            //We use radius to calculate the length of each of the sides
            int length = (int)(radius * 2 * Math.Sin(Math.PI / sides));

            //Caclulate new centre point

            //Copy of the first point
            Point temp_point1 = new Point(point1.X, point1.Y);
            //A second temporal copy that we will manipulate at the beginning of every loop
            Point temp_point2 = new Point(point1.X, point1.Y);
            for (int i = 0; i <= sides; i++)
            {
                double angle = 2 * Math.PI * (i + 1) / sides;
                temp_point2 = new Point((int)(centre(point1, point3).X + length * Math.Cos(angle)), (int)(centre(point1, point3).Y + length * Math.Sin(angle)));
                g.DrawLine(blackPen, temp_point1.X, temp_point1.Y, temp_point2.X, temp_point2.Y);
                temp_point1 = new Point(temp_point2.X, temp_point2.Y);
            }
            Pen deletePen = new Pen(Color.White);
            g.DrawLine(deletePen, point1.X, point1.Y, (int)(centre(point1, point3).X + length * Math.Cos(2 * Math.PI * 1 / sides)), (int)(centre(point1, point3).Y + length * Math.Sin(2 * Math.PI * 1 / sides)));

        }
        public override void mirror(Graphics g, Pen blackPen)
        {
            //Mirror could not be implemented for this shape neither for ellipse

        }
    }
}


