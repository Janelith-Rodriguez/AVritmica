using AutoMapper;
using AVritmica.BD.Data.Entity;
using AVritmica.Shared.DTO;

namespace AVritmica.Server.Util
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Ejemplo
            // CreateMap<CrearSourceDTO, Source>();
            
            //Carrito
            CreateMap<Carrito, CrearCarritoDTO>().ReverseMap();
            CreateMap<CrearCarritoDTO, Carrito>().ReverseMap();

            //CarritoProducto
            CreateMap<CarritoProducto, CrearCarritoProductoDTO>().ReverseMap();
            CreateMap<CrearCarritoProductoDTO, CarritoProducto>().ReverseMap();

            //Categoria
            CreateMap<Categoria, CrearCategoriaDTO>().ReverseMap();
            CreateMap<CrearCategoriaDTO, Categoria>().ReverseMap();
            CreateMap<CategoriaDTO, Categoria>().ReverseMap();

            //Compra
            CreateMap<Compra, CrearCompraDTO>().ReverseMap();
            CreateMap<CrearCompraDTO, Compra>().ReverseMap();
            
            //CompraDetalle
            CreateMap<CompraDetalle, CrearCompraDetalleDTO>().ReverseMap();
            CreateMap<CrearCompraDetalleDTO, CompraDetalle>();
            
            //Consulta
            CreateMap<Consulta, CrearConsultaDTO>().ReverseMap();
            CreateMap<CrearConsultaDTO, Consulta>().ReverseMap();
            //Pago
            CreateMap<Pago, CrearPagoDTO>().ReverseMap();
            CreateMap<CrearPagoDTO, Pago>().ReverseMap();
            
            //Producto
            CreateMap<Producto, CrearProductoDTO>().ReverseMap();
            CreateMap<CrearProductoDTO, Producto>().ReverseMap();
            CreateMap<ProductoDTO, Producto>().ReverseMap();

            //StockMovimiento
            CreateMap<StockMovimiento, CrearStockMovimientoDTO>().ReverseMap();
            CreateMap<CrearStockMovimientoDTO, StockMovimiento>().ReverseMap();
            
            //Usuario
            //CreateMap<Usuario, CrearUsuarioDTO>().ReverseMap();
        }
    }
}
