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
            //CreateMap<Carrito, CrearCarritoDTO>().ReverseMap();
            CreateMap<CrearCarritoDTO, Carrito>();

            //CarritoProducto
            //CreateMap<CarritoProducto, CrearCarritoProductoDTO>().ReverseMap();
            CreateMap<CrearCarritoProductoDTO, CarritoProducto>();

            //Categoria
            //CreateMap<Categoria, CrearCategoriaDTO>().ReverseMap();
            CreateMap<CrearCategoriaDTO, Categoria>();

            //Compra
            //CreateMap<Compra, CrearCompraDTO>().ReverseMap();
            CreateMap<CrearCompraDTO, Compra>();
            
            //CompraDetalle
            //CreateMap<CompraDetalle, CrearCompraDetalleDTO>().ReverseMap();
            CreateMap<CrearCompraDetalleDTO, CompraDetalle>();
            
            //Consulta
            CreateMap<Consulta, CrearConsultaDTO>();

            //Pago
            //CreateMap<Pago, CrearPagoDTO>().ReverseMap();
            CreateMap<CrearPagoDTO, Pago>();
            
            //Producto
            //CreateMap<Producto, CrearProductoDTO>().ReverseMap();
            CreateMap<CrearProductoDTO, Producto>();
            
            //StockMovimiento
            //CreateMap<StockMovimiento, CrearStockMovimientoDTO>().ReverseMap();
            CreateMap<CrearStockMovimientoDTO, StockMovimiento>();
            
            //Usuario
            //CreateMap<Usuario, CrearUsuarioDTO>().ReverseMap();
        }
    }
}
