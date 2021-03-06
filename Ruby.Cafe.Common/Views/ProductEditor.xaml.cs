using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ruby.Cafe.Model;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ruby.Cafe.Common.Screens
{
    /// <summary>
    /// Every cafe/restaurant/cafe kind businesses should sell products to sustain their lives. ProductEditor pages provides to add/remove/edit products. There are just few operations in this page to manipulate products: Add Product, Remove Product, Edit Serving, Add Category, Remove Category, Add Serving, Remove Serving, Refresh Lists, Pause, DisplaySucessMessage, ClearUI 
    /// </summary>
    public partial class ProductEditor : Page
    {
        private struct TreeItem
        {
            Product product;
            Category category;
        }

        #region Variables
        public Ruby.Cafe.Common.History HistoryInstance;
        private Database.IDatabase db;
        public Employee AccessedEmployee;

        public List<Product> ProductsInstance;
        public List<Category> CategoriesInstance;

        private Product __selected;
        private Product ActiveProduct
        {
            get
            {
                return __selected;
            }
            set
            {

                ServingLister.Items.Clear();

                if (value == null)
                    return;

                ProductNameBox.Text = value.Name;
                CategoryName.Text = value.category.Name;
                TaxBox.Text = value.Tax.ToString();
                Barcode.Text = value.Barcode;

                if(value.Servings != null)
                foreach (var serving in value.Servings)
                    ServingLister.Items.Add(serving);

                __selected = value;
            }
        }

        private bool SavedCurrentProduct = true;

        #endregion

        #region Methods

        /// <summary>
        /// Constructs the class
        /// </summary>
        public ProductEditor(Database.IDatabase database, History HistoryInstance)
        {
            this.db = database;
            this.HistoryInstance = HistoryInstance;

            InitializeComponent();

            using (Ruby.Serialization.Settings setings = new Ruby.Serialization.Settings(true))
            {
            this.Width = setings.Width;
            this.Height = setings.Height - 140;
            }
        }

        /// <summary>
        /// Creates a product
        /// </summary>
        /// <param name="Name">Product Name</param>
        /// <param name="Category">Category name that product will be displayed under it</param>
        /// <param name="TaxPercent">Percent of the tax which will be cut from the price</param>
        /// <param name="Barcode">Barcode if is there a barcode (Not required)</param>
        /// <returns></returns>
        public Product CreateProduct(string Name, string Category, double TaxPercent, string Barcode)
        {
            Product product = new Product();

            try
            {
                product.Name = Name;
                product.category = CategoriesInstance.Find(c => c.Name == Category);
                product.Tax = TaxPercent;
                product.Barcode = Barcode;
            }
            catch (Exception)
            {
                product.Tax = 0.0;
            }

            if (ProductsInstance.Find(p => p.Name == product.Name) != null)
                return null;

            return product;
        }

        /// <summary>
        /// Removes a selected product from the list
        /// </summary>
        public void RemoveProduct()
        {
            if (ActiveProduct == null || !(Producter.SelectedItem is string))
                return;
        }

        /// <summary>
        /// Creates Quantities that will be used on the ticket page
        /// </summary>
        /// <returns>Quantity reference</returns>
        public ServingItem AddServing()
        {
                int Quantity = 0;
                decimal Price = 0.0m;

                try
                {
                    Quantity = int.Parse(QuantityBox.Text);
                    Price = decimal.Parse(PriceBox.Text);
                }
                catch (Exception)
                {
                    Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.    MB_NumberedRequiredAreaTitle, string.Format(Ruby.Resources.Localization.MB_NumberedRequiredAreaMessage, TaxBox.Text), MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                return new ServingItem()
                {
                    Serving = ServingBox.Text,
                    Quantity = Quantity,
                    Price = Price
                };

        }

        public bool RemoveServing()
        {
            if (ServingLister.SelectedIndex == -1 || ServingLister.SelectedItem == null)
                return false;

            ServingItem si = (ServingItem)ServingLister.Items[ServingLister.SelectedIndex];
            db.DeleteServing(si.Serving,ActiveProduct.Name);
            ServingLister.Items.RemoveAt(ServingLister.SelectedIndex);

            return true;
        }

        /// <summary>
        /// When an error occurs, an error messages shows up
        /// </summary>
        /// <param name="MessageToDisplay"></param>
        /// <param name="font"></param>
        /// <param name="foreground"></param>
        /// <param name="StartLoc"></param>
        /// <exception cref="Exception"></exception>
        private void DisplaySuccessMessage(string MessageToDisplay, FontFamily font, Color foreground, VerticalAlignment StartLoc)
        {
            Thickness End;

            double Offset = (this.ActualHeight * 20) / 100;

            TextBlock tb = new TextBlock();
            tb.Foreground = new SolidColorBrush(foreground);
            tb.Text = MessageToDisplay;
            tb.FontFamily = font;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.FontSize = 24;
            if (StartLoc == VerticalAlignment.Top)
            {
                tb.Margin = new Thickness(0, 0 - 60, 0, 0);
                End = new Thickness(0, Offset, 0, 0);
            }
            else if (StartLoc == VerticalAlignment.Bottom)
            {
                tb.Margin = new Thickness(0, this.ActualHeight + 60, 0, 0);
                End = new Thickness(0, this.ActualHeight - Offset, 0, 0);
            }
            else
                throw new Exception("Only Top or Bottom alignments are allowed");
            tb.TextAlignment = TextAlignment.Center;

            Wrapper.Children.Add(tb);

            var mAnim = new System.Windows.Media.Animation.ThicknessAnimation(tb.Margin, End, new Duration(TimeSpan.FromSeconds(1)));
            mAnim.Completed += (s, e) =>
            {
                var t = new System.Threading.Thread(() =>
                {
                    System.Threading.Thread.Sleep(2000);
                    Dispatcher.Invoke(() =>
                    {
                        var oAnim = new System.Windows.Media.Animation.DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.5)));
                        tb.BeginAnimation(TextBlock.OpacityProperty, oAnim);
                    });
                });
                t.Start();
            };

            tb.BeginAnimation(TextBlock.MarginProperty, mAnim);
        }

        /// <summary>
        /// Clear UI values to default!
        /// </summary>
        private void ClearUI()
        {
            Producter.Items.Clear();
            ServingLister.Items.Clear();


            ProductNameBox.Text = Ruby.Resources.Localization.ProductEditor_DefaultProductNameTxt;
            CategoryName.Text = Ruby.Resources.Localization.ProductEditor_DefaultCategoryNameTxt;
            TaxBox.Text = Ruby.Resources.Localization.ProductEditor_DefaultTaxTxt;
            Barcode.Text = Ruby.Resources.Localization.ProductEditor_DefaultBarcodeTxt;
            ServingLister.Columns[0].Header = Ruby.Resources.Localization.ProductEditor_DefaultServingTxt;
            ServingLister.Columns[1].Header= Ruby.Resources.Localization.ProductEditor_DefaultQuantityTxt;
            ServingLister.Columns[2].Header = Ruby.Resources.Localization.ProductEditor_DefaultPriceTxt;

        }

        private void PauseFor(double sec, UIElement Element)
        {
            Element.IsEnabled = false;

            new System.Threading.Thread(() => {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(sec));
                Element.IsEnabled = true;
            }).Start();
        }

        /// <summary>
        /// Clears the treeviewer and listbox, and then sets the proper items into them
        /// </summary>
        private void RefreshList()
        {
            Producter.Items.Clear();

            ActiveProduct = null;

            if (CategoriesInstance.Count != 0 || ProductsInstance.Count != 0)
                foreach (var item in CategoriesInstance)
                {
                    var CategoryTab = new TreeViewItem();
                    CategoryTab.Header = item;
                    CategoryTab.Style = (Style)(this.Resources["CategoryTemplate"]);
                    foreach (var p in ProductsInstance.Where(p => p.category.Name == item.Name))
                    CategoryTab.Items.Add(p);
                    CategoryTab.IsExpanded = true;

                    Producter.Items.Add(CategoryTab);
                }
        }

        #endregion

        #region Events

        /// <summary>
        /// This event Happens when the page is loaded
        /// </summary>
        /// <param name="sender">Page</param>
        /// <param name="e">Routed Event Args</param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.PRODUCTEDITOR, MessageType.NOTIFICATION, Ruby.Resources.Localization.NOTIF_AccessedToScreen);

            CategoriesInstance = db.GetCategoryList();
            ProductsInstance = db.GetProductList();

            ClearUI();

            RefreshList();
        }

        /// <summary>
        /// Happens when the page is unloaded
        /// </summary>
        /// <param name="sender">Page</param>
        /// <param name="e">Routed Event Args</param>
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearUI();
        }

        /// <summary>
        /// Saves the changes of the product, if the product doesn't exist creates
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Event argument</param>        
        private void SaveProduct(object sender, RoutedEventArgs e)
        {
            /*
             * 1.Check if nameboxes are null
             * 2. Check if the product exists
             */

            if (!(
                string.IsNullOrWhiteSpace(ProductNameBox.Text) &&
                string.IsNullOrWhiteSpace(CategoryName.Text) &&
                string.IsNullOrWhiteSpace(TaxBox.Text)
                ))
            {
               
                if (double.TryParse(TaxBox.Text, out double tax))
                {
                Product p = CreateProduct(ProductNameBox.Text,CategoryName.Text,tax,Barcode.Text);

                    if (p == null) //Means there exists named such a product
                    {
                        PauseFor(3,SaveProductBtn);
                        PauseFor(3,RemoveProductBtn);

                        if (Producter.SelectedItem == null)
                        {
                            DisplaySuccessMessage(Ruby.Resources.Localization.MB_AlreadyExistsMessage, new FontFamily("Segoe UI"), Color.FromRgb(138, 7, 7), VerticalAlignment.Bottom);
                            return;
                        }

                        //Create category if not exists
                        if (!CategoriesInstance.Exists(c => c.Name == p.category.Name))
                        {
                            Category cat = new Category();

                            cat.Name = CategoryName.Text;
                            CategoriesInstance.Add(cat);
                            db.AddCategory(cat.Name);

                            Producter.Items.Add(new TreeViewItem() { Header = cat.Name, Style = (Style)(this.Resources["CategoryTemplate"]) });

                            p.category = cat;

                            HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.PRODUCTEDITOR, MessageType.PROCESS, string.Format(Ruby.Resources.Localization.PROCESS_CategoryCreated, cat.Name));
                        }

                        db.UpdateProduct(ProductNameBox.Text,CategoryName.Text,tax,Barcode,);

                        return;
                    }

                    p.Servings = new List<ServingItem>();

                    if (ServingLister.Items.Count > 0 && ActiveProduct == null)
                        foreach(ServingItem item in ServingLister.Items)
                        p.Servings.Add(item);

                    //Create category if not exists
                    if (!CategoriesInstance.Exists(c=> c.Name == p.category.Name))
                    {
                        Category cat = new Category();

                        cat.Name = CategoryName.Text;
                        CategoriesInstance.Add(cat);
                        db.AddCategory(cat.Name);

                        Producter.Items.Add(new TreeViewItem() {Header = cat.Name, Style = (Style)(this.Resources["CategoryTemplate"]) });

                        p.category = cat;

                        HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.PRODUCTEDITOR, MessageType.PROCESS, string.Format(Ruby.Resources.Localization.PROCESS_CategoryCreated, cat.Name));
                    }

                    foreach (TreeViewItem tvi in Producter.Items)
                    {
                        if (tvi.Header.ToString() == p.category.Name)
                            tvi.Items.Add(p);
                    }

                    ProductsInstance.Add(p);
                    db.AddProduct(p.Name,p.category.Name,p.Tax,p.Barcode);

                    HistoryInstance.SendMessage(AccessedEmployee,ScreenEnum.PRODUCTEDITOR,MessageType.PROCESS,string.Format(Ruby.Resources.Localization.PROCESS_ProductCreated,p.Name));
                    HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.PRODUCTEDITOR, MessageType.PROCESS, string.Format(Ruby.Resources.Localization.PROCESS_ProductCategorySetted,p.Name,p.category.Name));
                }
            }
            else
            {
                Ruby.Cafe.Common.Controls.MessageBox.ShowMessageBox(Ruby.Resources.Localization.MB_FillRequiredAreasTitle, Ruby.Resources.Localization.MB_FillRequiredAreasMessage, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SavedCurrentProduct = true;

            ActiveProduct = ProductsInstance[ProductsInstance.Count - 1];
        }

        /// <summary>
        /// Saves a product with new properties over the active product
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Routed event args</param>
        private void EditSelectedProduct(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Removes the product from list,serialized file and grid which is located on the producter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveSelectedProduct(object sender, RoutedEventArgs e)
        {
            if (ActiveProduct == null) return;

            int CategoryIndex = CategoriesInstance.IndexOf(ActiveProduct.category);
            
            ((TreeViewItem)Producter.Items[CategoryIndex]).Items.Remove(ActiveProduct.Name);
            
            //Delete the category if it has zero product inside of its items
            if (((TreeViewItem)Producter.Items[CategoryIndex]).Items.Count == 0)
            {
                Producter.Items.RemoveAt(CategoryIndex);
                db.DeleteCategory(CategoriesInstance[CategoryIndex].Name);
                CategoriesInstance.RemoveAt(CategoryIndex);
            }

            String CProductName = ActiveProduct.Name;

            db.DeleteProduct(ActiveProduct.Name);
            ProductsInstance.Remove(ActiveProduct);

            HistoryInstance.SendMessage(AccessedEmployee, ScreenEnum.PRODUCTEDITOR, MessageType.PROCESS,string.Format(Ruby.Resources.Localization.PROCESS_ProductDeleted,CProductName));

            RefreshList();

            ActiveProduct = null;
        }
        /// <summary>
        /// Create copy of the selected product and allows to change that product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopySelectedProduct(object sender, RoutedEventArgs e)
        {
            if (Producter.SelectedValue == null) return;
        }
            
        /// <summary>
        /// Adds a row on the quantity data grid list 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddServing(object sender,MouseEventArgs e)
        {
            if (ActiveProduct == null) return;
            if (!(string.IsNullOrWhiteSpace(ServingBox.Text) && string.IsNullOrWhiteSpace(QuantityBox.Text) && string.IsNullOrWhiteSpace(PriceBox.Text)) && decimal.TryParse(PriceBox.Text,out decimal price)  && int.TryParse(QuantityBox.Text,out int quantity))
            {
                ServingItem qr = AddServing();
            
            ActiveProduct.Servings.Add(qr);
            ServingLister.Items.Add(qr);
            db.AddServing(qr.Serving, qr.Quantity, qr.Price,ActiveProduct.Name);
            }
        }

        /// <summary>
        /// Removes a row on the quantity data grid list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveServing(object sender, MouseEventArgs e)
        {
            if (ServingLister.SelectedItem == null && ServingLister.SelectedIndex < 0) return;

            ServingItem qr = (ServingItem)ServingLister.SelectedItem;
            ProductsInstance.Find(p=> p.Name == ActiveProduct.Name).Servings.Remove(qr);
            db.DeleteServing(qr.Serving,ActiveProduct.Name);
            ServingLister.Items.Remove(ServingLister.SelectedItem);
        }

        /// <summary>
        /// Happens when 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickAProduct(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                ActiveProduct = ProductsInstance.Find(p=> p.Name == Producter.SelectedItem.ToString());
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Changed(object sender, TextChangedEventArgs e)
        => SavedCurrentProduct = false;
        
        private void AvoidChar(object sender, KeyEventArgs e)
        {
         int ascii = KeyInterop.VirtualKeyFromKey(e.Key);

            if (!(ascii == 46 || ascii > 48 && ascii < 57))
                return;

        }

        #endregion

    }
}
