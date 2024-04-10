using Microsoft.Data.SqlClient;

namespace sqlCsharp
{
    public class DataImporter
    {
        public void ImportDataFromTextFile()
        {
            string filePath = @"D:\Products.txt"; // რასაც წაიკითხავს.
            List<Category> categories = new List<Category>();

            if (File.Exists(filePath))
            {

                string insertCategory = "INSERT INTO Categories (Name, IsActive, CreateDate) OUTPUT INSERTED.CategoryID VALUES (@Name, @IsActive, @CreateDate)";

                // SELECT SCOPE_IDENTITY(); -- ეს მაძლევს high values (categoryID = 253...);
                // OUTPUT INSERTED.CategoryID -- ეს არ.

                string insertProduct = "INSERT INTO Products (CategoryID, Code, Name, Price, IsActive, CreateDate) VALUES (@CategoryID, @Code, @Name, @Price, @IsActive, @CreateDate)";
                string selectCategory = "SELECT CategoryID FROM Categories WHERE Name = @Name";

                string connectionString = "Server=DESKTOP-FGUKH5T; Database=FirstAdonetSecondTry; Integrated Security=true; TrustServerCertificate=true";

               
                //using (IConnectionToSQL connection = new ConnectionToSQL(connectionString)) // interface(AKA კონტრაქტი).
                //{
                //    connection.Connection();     // ვერ ვიყენებ ნორმალურად .

                    using (StreamReader reader = new StreamReader(filePath)) // ფაილის წაკითხვა.
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split('\t');

                            string categoryName = parts[0];
                            bool isActive = parts[1] == "1";
                            string productID = parts[2];
                            string productName = parts[3];
                            double price = double.Parse(parts[4]);
                            bool IsActiveP = parts[5] == "1";

                            Category category = new Category(categoryName, isActive);
                            categories.Add(category);
                            category.Products.Add(new Product(productID, productName, price, isActive));

                            // ვამოწმებთ თუ category არსებობს უკვე სანამ ახალს ჩავამა.
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                conn.Open();
                                SqlCommand selectCommand = new SqlCommand(selectCategory, conn);
                                selectCommand.Parameters.AddWithValue("@Name", categoryName);
                                int categoryID = (int?)selectCommand.ExecuteScalar() ?? 0;


                                if (categoryID == 0)
                                {
                                    // category არ არსებობს და ვაinsert-ებთ
                                    SqlCommand categoryCommand = new SqlCommand(insertCategory, conn);
                                    categoryCommand.Parameters.AddWithValue("@Name", categoryName);
                                    categoryCommand.Parameters.AddWithValue("@IsActive", isActive);
                                    categoryCommand.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                    categoryCommand.Parameters.AddWithValue("@UpdateDate", DateTime.Today);
                                    categoryID = Convert.ToInt32(categoryCommand.ExecuteScalar());
                                }

                                SqlCommand productCommand = new SqlCommand(insertProduct, conn);
                                productCommand.Parameters.AddWithValue("@CategoryID", categoryID);
                                productCommand.Parameters.AddWithValue("@Code", productID);
                                productCommand.Parameters.AddWithValue("@Name", productName);
                                productCommand.Parameters.AddWithValue("@Price", price);
                                productCommand.Parameters.AddWithValue("@IsActive", IsActiveP);
                                productCommand.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                productCommand.Parameters.AddWithValue("@UpdateDate", DateTime.Today);
                                productCommand.ExecuteNonQuery();

                                Console.WriteLine("Data inserted successfully.");
                            }
                        }
                    
                    Console.WriteLine();
                    Print(categories);
                }
            }
        }

        public static void Print(List<Category> categories) // print ფუნქცია.
        {
            foreach (Category item in categories)
            {
                Console.ForegroundColor = ConsoleColor.Green; // ფერი.
                Console.WriteLine($"Category: {item.Name}, IsActive: {item.IsActive}");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Blue; 
                Console.WriteLine("Products: ");

                foreach (Product items in item.Products)
                {
                    Console.WriteLine($"\t{items.ProductID}, {items.Name}, {items.Price}, {items.IsActive}");
                }
                Console.ResetColor();

                Console.WriteLine();
            }
        }
    }
}