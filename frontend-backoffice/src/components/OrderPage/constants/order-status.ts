export enum OrderStatus {
    RECIBIDO =  "recibido",
    PROCESANDO = 'procesando',
    PENDING = 'pending',
    PREPARANDO = 'preparando',
    EN_CAMINO = 'en_camino',
    ENTREGADO = 'entregado',
    FALLA_ENTREGA = 'falla_entrega',
    CANCELADO = 'cancelado',
}
  
export const ORDER_STATUS_OPTIONS = [
    { label: 'Pedido Recibido', value: OrderStatus.RECIBIDO },
    { label: 'Procesando', value: OrderStatus.PROCESANDO },
    { label: 'Pending', value: OrderStatus.PENDING },
    { label: 'Preparando', value: OrderStatus.PREPARANDO },
    { label: 'En camino', value: OrderStatus.EN_CAMINO },
    { label: 'Entregado', value: OrderStatus.ENTREGADO },
    { label: 'Entrega fallida', value: OrderStatus.FALLA_ENTREGA },
    { label: 'Cancelado', value: OrderStatus.CANCELADO },
];

export const ORDER_STATUS_COLORS: Record<string, string> = {
    recibido: 'bg-primary',
    Pending: 'bg-info',
    preparando: 'bg-warning',
    en_camino: 'bg-secondary',
    entregado: 'bg-success',
    falla_entrega: 'bg-danger',
    cancelado: 'bg-dark',
  };
  