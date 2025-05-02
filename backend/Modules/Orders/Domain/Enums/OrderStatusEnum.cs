namespace Backend.Modules.Orders.Domain.Enums {

    public enum OrderStatusEnum
    {
        Recibido = 1,
        Procesando = 2,
        Preparando = 3,
        EnCamino = 4,
        Entregado = 5,
        FallaEntrega = 6,
        Cancelado = 7
    }
}
