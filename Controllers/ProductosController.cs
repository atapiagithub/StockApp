using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StockApp.Models;
using Microsoft.Data.Sqlite;
using System.Data;

namespace StockApp.Controllers
{
    public class ProductosController : Controller
    {
        IConfiguration Configuracion;

        public ProductosController(IConfiguration configuration)
        {
            Configuracion = configuration;
        }

        // GET: ProductosController
        public ActionResult Index()
        {

            string szConection = Configuracion["ConexionBase"];
            DataTable oTablaDatos = new DataTable();

            using (SqliteConnection oConexionSQL = new SqliteConnection(szConection))
            {
                using (SqliteCommand oComandoSQL = new SqliteCommand("SELECT * FROM PRODUCTOS", oConexionSQL))
                {
                    oConexionSQL.Open();
                    using (SqliteDataReader oReaderSQL = oComandoSQL.ExecuteReader())
                    {
                        oTablaDatos.Load(oReaderSQL);
                    }
                    oConexionSQL.Close();
                }
            }

            List<ProductoModel> oListaProductos = new List<ProductoModel>();
            
            foreach (DataRow item in oTablaDatos.Rows)
            {                
                ProductoModel oProducto = new ProductoModel();

                int iRetorno;
                if (int.TryParse(item["Cantidad"].ToString(), out iRetorno))
                {
                    oProducto.Cantidad = iRetorno;
                }
                else
                {
                    oProducto.Cantidad = 0;
                }

                oProducto.IdProducto = int.Parse(item["IdProducto"].ToString());
                oProducto.Codigo = item["Codigo"].ToString(); 
                oProducto.Descripcion = item["Descripcion"].ToString();
                oListaProductos.Add(oProducto);
            }
            

            return View(oListaProductos);
        }

        // GET: ProductosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductosController/Create
        public ActionResult Create()
        {            
            return View();
        }

        // POST: ProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductoModel oProducto)
        {
            string szConection = Configuracion["ConexionBase"];
            SqliteConnection oConexionSQL = new SqliteConnection(szConection);

            try
            {
                SqliteCommand oComandoSQL = new SqliteCommand("INSERT INTO Productos (Codigo, Descripcion, Cantidad)  VALUES (@Codigo, @Descripcion, @Cantidad);", oConexionSQL);

                oComandoSQL.Parameters.AddWithValue("@Codigo", oProducto.Codigo);
                oComandoSQL.Parameters.AddWithValue("@Descripcion", oProducto.Descripcion);
                oComandoSQL.Parameters.AddWithValue("@Cantidad", oProducto.Cantidad);                

                oConexionSQL.Open();
                oComandoSQL.ExecuteNonQuery();
                oConexionSQL.Close();

                return RedirectToAction(nameof(Index));
            }
            catch (SqliteException sqlex)
            {
                ViewBag.Error = "Ocurrió un error en la conexion con la base de datos. Por favor intente más tarde";
                ViewBag.ConsoleError = sqlex.Message;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrio un error. Por favor intente mas tarde";
                ViewBag.ConsoleError = ex.Message;
                return View();
            }
            finally
            {
                if (oConexionSQL.State == ConnectionState.Open)
                {
                    oConexionSQL.Close();
                }
                oConexionSQL.Dispose();
            }
        }

        // GET: ProductosController/Edit/5
        public ActionResult Edit(int id)
        {

            string szConection = Configuracion["ConexionBase"];
            DataTable oTablaDatos = new DataTable();

            SqliteConnection oConexionSQL = new SqliteConnection(szConection);
            SqliteCommand oComandoSQL = new SqliteCommand("SELECT * FROM PRODUCTOS WHERE IDPRODUCTO = @IdProducto  ", oConexionSQL);
            oComandoSQL.Parameters.AddWithValue("@IdProducto", id);

            oConexionSQL.Open();
            SqliteDataReader oReaderSQL = oComandoSQL.ExecuteReader();
            oTablaDatos.Load(oReaderSQL);
            oConexionSQL.Close();

            DataRow item = oTablaDatos.Rows[0];

            ProductoModel oProducto = new ProductoModel();

            int iRetorno;
            if (int.TryParse(item["Cantidad"].ToString(), out iRetorno))
            {
                oProducto.Cantidad = iRetorno;
            }
            else
            {
                oProducto.Cantidad = 0;
            }

            oProducto.IdProducto = int.Parse(item["IdProducto"].ToString());
            oProducto.Codigo = item["Codigo"].ToString();
            oProducto.Descripcion = item["Descripcion"].ToString();


            return View(oProducto);
        }

        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductoModel oProducto)
        {
            string szConection = Configuracion["ConexionBase"];
            SqliteConnection oConexionSQL = new SqliteConnection(szConection);

            try
            {
                string szUpdate = " UPDATE PRODUCTOS SET ";
                szUpdate += " CODIGO = @Codigo, ";
                szUpdate += " DESCRIPCION = @Descripcion, ";
                szUpdate += " CANTIDAD = @Cantidad ";
                szUpdate += " WHERE IdProducto = @IdProducto ";

                SqliteCommand oComandoSQL = new SqliteCommand(szUpdate, oConexionSQL);

                oComandoSQL.Parameters.AddWithValue("@Codigo", oProducto.Codigo);
                oComandoSQL.Parameters.AddWithValue("@Descripcion", oProducto.Descripcion);
                oComandoSQL.Parameters.AddWithValue("@Cantidad", oProducto.Cantidad);
                oComandoSQL.Parameters.AddWithValue("@IdProducto", id);

                oConexionSQL.Open();
                oComandoSQL.ExecuteNonQuery();
                oConexionSQL.Close();

                return RedirectToAction(nameof(Index));
            }
            catch (SqliteException sqlex)
            {
                ViewBag.Error = "Ocurrió un error en la conexion con la base de datos. Por favor intente más tarde";
                ViewBag.ConsoleError = sqlex.Message;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrio un error. Por favor intente mas tarde";
                ViewBag.ConsoleError = ex.Message;
                return View();
            }
            finally
            {
                if (oConexionSQL.State == ConnectionState.Open)
                {
                    oConexionSQL.Close();
                }
                oConexionSQL.Dispose();
            }
        }

        // GET: ProductosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
