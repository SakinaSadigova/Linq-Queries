using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Diags02.Entity;

namespace Diags02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Click_Click(object sender, EventArgs e)
        {

            //----------------Question 1
            lblQuery.Text = "1.UnitPrice dəyəri maksimum olan Album title-ı tapın.";
            Result.Text = " \r\n Cavab : ";
            var maxUnitPrice = LINQ_Entity.OrderDetails.OrderByDescending(x => x.UnitPrice).First();            
            var albumQuery = LINQ_Entity.Albums.Where(x => x.AlbumId == maxUnitPrice.AlbumId);

            foreach (var album in albumQuery)
            {
                Result.Text += album.Title + "\r\n";
            }
            Result.Text += "\r\n";
            dgwResult.DataSource = albumQuery.ToList();           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //-----------------Question 2
            lblQuery.Text = "2.Hər hansı Genre dəyərinə sahib olan albomların siyahısını çıxarın.";
            Result.Text = " \r\n Cavab : ";
            var genreId = LINQ_Entity.Genres.Where(y => y.Name == "Pop").Select(y => y.GenreId).FirstOrDefault();
            var albumDetails = LINQ_Entity.Albums.Where(y => y.GenreId == genreId);
            foreach (var albumDetail in albumDetails)
            {
                Result.Text += albumDetail.AlbumId + "." + albumDetail.Title + "\\" + albumDetail.Price + "\\" + albumDetail.AlbumArtUrl + "\r\n";
            }
            dgwResult.DataSource = albumDetails.ToList();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //-----------------Question 3
            lblQuery.Text = "3.Son 1 ayda satılan albom sayı.";
            Result.Text = "\r\n Cavab :  ";
            var orders = LINQ_Entity.Orders.Where(x => x.OrderDate >= new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day));
            Result.Text += orders.Count() + "\r\n";
            dgwResult.DataSource = orders.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //-----------------Question 4
            lblQuery.Text = "4.Son bir ayda qazanılan pul miqdarı";
            Result.Text = "\r\n Cavab :  ";
            var Total = 0;
            var orderIds = LINQ_Entity.Orders.Where(x => x.OrderDate <= DateTime.Today && x.OrderDate >= new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, DateTime.Today.Day)).Select(y => y.OrderId);

            var detailQuery = from detail in LINQ_Entity.OrderDetails
                                 from orderId in orderIds
                                 where detail.OrderId == orderId
                                 select detail;
            foreach (var detail in detailQuery)
            {
                Total += detail.UnitPrice;
            }
            Result.Text += Total + "\r\n";
            dgwResult.DataSource = detailQuery.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //-----------------Question 5
            lblQuery.Text = "5.Seçilən Artist adına uyğun gələn bütün Orderlər tapın(Meselen Zohrali)";
            Result.Text = "\r\n Cavab :  ";

            var artistIdQuery = from artist in LINQ_Entity.Artists
                                where artist.Name == "Zohrali"
                                select artist.ArtistId;

            var albumIdQuery = from album in LINQ_Entity.Albums
                               from artistId in artistIdQuery
                               where album.ArtistId == artistId
                               select album.AlbumId;

            var orderIdQuery = from orderDetail in LINQ_Entity.OrderDetails
                               from albumId in albumIdQuery
                               where orderDetail.AlbumId == albumId
                               select orderDetail.OrderId;

            var ordersQuery = from order in LINQ_Entity.Orders
                              from orderId in orderIdQuery
                              where order.OrderId == orderId
                              select order;
            foreach (var order in ordersQuery)
            {
                Result.Text += order
                    .OrderId + ". " + order
                    .UserName + " \\ " + order
                    .FirstName + " \\ " + order
                    .LastName + " \\ " + order
                    .Address + " \\ " + order
                    .Phone + " \\ ";
            }
            Result.Text += "\r\n";
            dgwResult.DataSource = ordersQuery.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lblQuery.Text = "6.Price dəyəri Avarage price dəyərindən böyük olan Albomlardan Title dəyəri ən uzun olan Albomun Satış miqdarını (Count) tapın";
            Result.Text = "\r\n Cavab :  ";
           
            var albumsQuery = from album in LINQ_Entity.Albums
                              where album.Price >( LINQ_Entity.Albums.Average(x => x.Price))
                              select album;

            var albumIdQuery = from album in albumsQuery
                               where album.Title.Length == albumsQuery.Max(x => x.Title.Length)
                               select album.AlbumId;

            var orderIdQuery = from orderDetail in LINQ_Entity.OrderDetails
                                   from albumId in albumIdQuery
                                   where orderDetail.AlbumId == albumId
                                   select orderDetail.OrderId;
            var orderQuery = from order in LINQ_Entity.Orders
                             from orderId in orderIdQuery
                             where order.OrderId == orderId
                             select order;
            Result.Text += orderQuery.Count();
            dgwResult.DataSource = orderQuery.ToList();


        }

        private void button6_Click(object sender, EventArgs e)
        {
            //-----------------Question 7
            lblQuery.Text = "7.Seçilən ölkədə ən çox satılan Genre adını tapın";
            Result.Text = "\r\n Cavab :  ";
            //country="Azerbaijan";
            var orderIdsQuery = from order in LINQ_Entity.Orders
                                where order.Country == "Azerbaijan"
                                select order.OrderId;

            var albumIdsQuery = from orderDetail in LINQ_Entity.OrderDetails
                                from orderId in orderIdsQuery
                                where orderDetail.OrderId == orderId &&
                                orderDetail.Quantity == (LINQ_Entity.OrderDetails.Max(x => x.Quantity))
                                select orderDetail.AlbumId;

            var GenreIdQuery = from album in LINQ_Entity.Albums
                               from albumId in albumIdsQuery
                               where album.AlbumId == albumId
                               select album.GenreId;

            var GenreQuery = from genre in LINQ_Entity.Genres
                             from genreId in GenreIdQuery
                             where genre.GenreId == genreId
                             select genre;
            foreach (var genre in GenreQuery)
            {
                Result.Text += genre.Name;
            }
            dgwResult.DataSource = GenreQuery.ToList();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //-----------------Question 8
            lblQuery.Text = "8.Adının uzunluğu 5-dən böyük olan Artistləri əlifba sırasına görə sıralayın";
            Result.Text = "\r\n Cavab :  ";

            var artistQuery = from artist in LINQ_Entity.Artists
                              where artist.Name.Length > 5
                              orderby artist.Name
                              select artist;
            dgwResult.DataSource = artistQuery.ToList();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //-----------------Question 9
            lblQuery.Text = "9.Seçilən şəhərə aid olan orderlərdən UnitPrice dəyəri 5 AZN dən yüksək olan Albom satışlarının cəmini tapın";
            Result.Text = "\r\n Cavab :  ";

            //City="Baki"
            var Total = 0;
            var orderIdQuery = from order in LINQ_Entity.Orders
                               where order.City == "Baki"
                               select order.OrderId;
            var orderDetailQuery = from orderDetail in LINQ_Entity.OrderDetails
                                   from orderId in orderIdQuery
                                   where orderDetail.OrderId == orderId && orderDetail.UnitPrice > 5
                                   select orderDetail;
            foreach (var orderDetail in orderDetailQuery)
            {
                Total += orderDetail.Quantity;
            }
            Result.Text += Total;
            dgwResult.DataSource = orderDetailQuery.ToList();

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }
    }
}
