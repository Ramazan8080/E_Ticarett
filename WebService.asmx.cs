using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
namespace E_Ticarett
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://elemegim.somee.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
   
    [ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        SqlConnection baglanti = new SqlConnection(ConfigurationManager.ConnectionStrings["baglantiismi"].ConnectionString);
        SqlCommand komut;



        [WebMethod]
        public void UyebilgisiGetir(string kul_adi,string sifre)
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            List<Kullanicilar> Kullanicilar = new List<Kullanicilar>();
            SqlCommand cmd = new SqlCommand("Select * from Uyeler where kullanici_adi=@kul_adi and sifre=@sifre ", baglanti);
            cmd.Parameters.Add("@kul_adi", SqlDbType.VarChar).Value = kul_adi;
            cmd.Parameters.Add("@sifre", SqlDbType.VarChar).Value = sifre;
            SqlDataReader read = cmd.ExecuteReader();
            try
            {

                while (read.Read())
                {
                    Kullanicilar kullanici = new Kullanicilar();
                    kullanici.kullanici_adi = read["kullanici_adi"].ToString();
                    kullanici.adres = read["adres"].ToString();
                    kullanici.eposta = read["eposta"].ToString();
                    kullanici.telefon = read["telefon"].ToString();
                    kullanici.sifre = read["sifre"].ToString();
                    kullanici.sifre_tekrar = read["sifre_tekrar"].ToString();
                    Kullanicilar.Add(kullanici);
                }
                    

                
                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Write(js.Serialize(Kullanicilar));
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                baglanti.Dispose();
                baglanti.Close();
            }


        }



        [WebMethod]
        public string KullaniciAdiKontrol(string kullanici_adi)
        {
            string sonuc="boş";
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
         
            SqlCommand cmd = new SqlCommand("Select kullanici_adi from musteriler where kullanici_adi=@Id", baglanti);
            cmd.Parameters.Add("@Id", SqlDbType.VarChar).Value = kullanici_adi;
            SqlDataReader okuyucu = cmd.ExecuteReader();
           
            while (okuyucu.Read())
            {
            if (kullanici_adi == okuyucu["kullanici_adi"].ToString())
            {
                sonuc= "BU KULLANICI ADI VERİ TABANINDA ZATEN KAYITLI";
            }
            else
            {
                SqlCommand cmd1 = new SqlCommand("insert into musteriler(kullanici_adi) values kullanici_adi(@Id)", baglanti);
                    cmd.Parameters.AddWithValue("@deger", kullanici_adi);
                    sonuc= "SONUC BAŞARILI";
               
            }
            }
           

           
            baglanti.Close();
            return sonuc;
        }






        [WebMethod]
        public bool UrunEkle(String urunadi, int fiyat,String aciklama, DateTime tarih,int begeni,int adet)
        {
            bool sonuc = false;
            SqlCommand cmd = new SqlCommand("Insert Into D_urunler(urun_adi,fiyat,aciklama,eklenme_tarihi,begeni_sayisi,adet) values(@urunadi,@fiyat,@aciklama,@ek_tarih,@begeni,@adet)", baglanti);
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                cmd.Parameters.Add("@urunadi", SqlDbType.VarChar).Value = urunadi;
                cmd.Parameters.Add("@fiyat", SqlDbType.Money).Value = fiyat;
                cmd.Parameters.Add("@aciklama", SqlDbType.VarChar).Value =aciklama;
                cmd.Parameters.Add("@ek_tarih", SqlDbType.SmallDateTime).Value = tarih;
                cmd.Parameters.Add("@begeni", SqlDbType.Int).Value = begeni;
                cmd.Parameters.Add("@adet", SqlDbType.Int).Value = adet;

                sonuc = Convert.ToBoolean(cmd.ExecuteNonQuery());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                baglanti.Dispose();
                baglanti.Close();
            }
            return sonuc;
        }




        [WebMethod]
        public bool UyeEkle(String kullanici_adi, String adres, String eposta, int telefon, String sifre, String sifre_tekrar)
        {
            bool sonuc = false;
            SqlCommand cmd = new SqlCommand("Insert Into Uyeler(kullanici_adi,adres,eposta,telefon,sifre,sifre_tekrar) values(@kullanici_adi,@adres,@eposta,@telefon,@sifre,@sifre_tekrar)", baglanti);
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                cmd.Parameters.Add("@kullanici_adi", SqlDbType.VarChar).Value = kullanici_adi;
                cmd.Parameters.Add("@adres", SqlDbType.VarChar).Value = adres;
                cmd.Parameters.Add("@eposta", SqlDbType.VarChar).Value = eposta;
                cmd.Parameters.Add("@telefon", SqlDbType.Int).Value = telefon;
                cmd.Parameters.Add("@sifre", SqlDbType.VarChar).Value = sifre;
                cmd.Parameters.Add("@sifre_tekrar", SqlDbType.VarChar).Value = sifre_tekrar;

                sonuc = Convert.ToBoolean(cmd.ExecuteNonQuery());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                baglanti.Dispose();
                baglanti.Close();
            }
            return sonuc;
        }







        [WebMethod]
        public bool BilgiGuncelle(String kullanici_adi, String adres, String eposta, int telefon, String sifre, String sifre_tekrar)
        {
            bool sonuc = false;
            SqlCommand cmd = new SqlCommand("Update Uyeler set kullanici_adi=@kullanici_adi,adres=@adres,eposta=@eposta,telefon=@telefon,sifre=@sifre,sifre_tekrar=@sifre_tekrar where kullanici_adi=@kullanici_adi", baglanti);
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                cmd.Parameters.Add("@kullanici_adi", SqlDbType.VarChar).Value = kullanici_adi;
                cmd.Parameters.Add("@adres", SqlDbType.VarChar).Value = adres;
                cmd.Parameters.Add("@eposta", SqlDbType.VarChar).Value = eposta;
                cmd.Parameters.Add("@telefon", SqlDbType.Int).Value = telefon;
                cmd.Parameters.Add("@sifre", SqlDbType.VarChar).Value = sifre;
                cmd.Parameters.Add("@sifre_tekrar", SqlDbType.VarChar).Value = sifre_tekrar;

                sonuc = Convert.ToBoolean(cmd.ExecuteNonQuery());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                baglanti.Dispose();
                baglanti.Close();
            }
            return sonuc;
        }






        [WebMethod]
        public void urungetir()
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            List<Urunler> urunlers = new List<Urunler>();
            SqlCommand cmd = new SqlCommand("Select * from D_urunler ", baglanti);
            SqlDataReader read = cmd.ExecuteReader();
            try
            {
               
                while (read.Read())
                {
                    Urunler urun = new Urunler();
                    urun.urun_id = read["urun_id"].ToString();
                    urun.urun_adi = read["urun_adi"].ToString();
                    urun.fiyat = read["fiyat"].ToString();
                    urun.aciklama = read["aciklama"].ToString();
                    urun.eklenme_tarihi = read["eklenme_tarihi"].ToString();
                    urun.begeni_sayisi = read["begeni_sayisi"].ToString();
                    urun.adet = read["adet"].ToString();
                    urunlers.Add(urun);

                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                Context.Response.Write(js.Serialize(urunlers));
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                baglanti.Dispose();
                baglanti.Close();
            }


        }




      /*  [WebMethod]
        public DataTable Kategori_sorgulama(int Kat_id,int urun_id,String kat_adi,String ust_kat)
        {
      

                if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd=new SqlCommand("Select kategoriler.kategori_id=@Kat_id,kategoriler.urun_id=@urun_id,kategoriler.kategori_adi=@kat_adi,kategoriler.ust_kategori=@ust_kat from kategoriler Inner Join D_urunler on kategoriler.urun_id=D_urunler.urun_id where kategoriler.kategori_id=Kat_id", baglanti);

            //SqlDataAdapter da = new SqlDataAdapter("Select D_urunler.urun_id,D_urunler.urun_adi,D_urunler.fiyat,D_urunler.aciklama,D_urunler.eklenme_tarihi,D_urunler.begeni,D_urunler.adet from D_urunler Inner Join kategoriler on kategoriler.urun_id=D_urunler.urun_id where kategoriler.kategori_id=@Kat_id", baglanti);
            cmd.Parameters.Add("@kategori_id", SqlDbType.Int).Value = Kat_id;
            cmd.Parameters.Add("@urun_id", SqlDbType.Int).Value = urun_id;
            cmd.Parameters.Add("@kategori_adi", SqlDbType.VarChar).Value = kat_adi;
            cmd.Parameters.Add("@ust_kategori", SqlDbType.VarChar).Value = ust_kat;

            DataTable dt = new DataTable("urun");
            da.Fill(dt);
            return dt;

        }*/
    }
}
