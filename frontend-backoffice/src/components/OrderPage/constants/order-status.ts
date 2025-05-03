import '../../../App.css';

export enum OrderStatus {
    Recibido = 1,
    Procesando = 2,
    Preparando = 3,
    EnCamino = 4,
    Entregado = 5,
    FallaEntrega = 6,
    Cancelado = 7
}
  
export const ORDER_STATUS_OPTIONS = [
    { value: OrderStatus.Recibido, label: 'Pedido Recibido' },
    { value: OrderStatus.Procesando, label: 'Procesando' },
    { value: OrderStatus.Preparando, label: 'Preparando' },
    { value: OrderStatus.Cancelado, label: 'Cancelado' },
    { value: OrderStatus.EnCamino, label: 'En Camino' },
    { value: OrderStatus.Entregado, label: 'Entregado' },
    { value: OrderStatus.FallaEntrega, label: 'Entrega Fallida' }
];
  
export const ORDER_STATUS_COLORS: Record<keyof typeof OrderStatus, string> = {
    Recibido: 'bg-light-primary',
    Procesando: 'bg-light-dark',
    Preparando: 'bg-light-warning',
    EnCamino: 'bg-light-info',
    Entregado: 'bg-light-success',
    FallaEntrega: 'bg-light-danger',
    Cancelado: 'bg-light-orange'
};

  