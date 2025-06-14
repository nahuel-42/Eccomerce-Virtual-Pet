import React, { useEffect, useState } from 'react';
import { Modal, Button } from 'react-bootstrap';
import { getOrderById } from '../../../../services/order.service';
import { ORDER_STATUS_OPTIONS } from '../../constants/order-status';
import StatusRenderer from '../StatusRenderer/StatusRenderer';


export default function OrderDetailModal({ orderId, show, onHide }) {
  const [order, setOrder] = useState(null);
  const [status, setStatus] = useState();

  useEffect(() => {
    if (show) {
      getOrderById(orderId).then((res) => {
        setOrder(res.data);
        setStatus(res.data.status);
      });
    }
  }, [orderId, show]);

  if (!order) return null;

  const statusLabel = ORDER_STATUS_OPTIONS.find(opt => opt.value === status)?.label || 'Desconocido';

  return (
    <Modal show={show} onHide={onHide} size="lg">
      <Modal.Header closeButton>
        <Modal.Title>Pedido #{order.id}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <p><strong>Dirección:</strong> {order.address}</p>
        <p><strong>Teléfono:</strong> {order.phone}</p>
        <p><strong>Cliente:</strong> {order.user?.name} ({order.user?.email})</p>
        <p><strong>Productos:</strong></p>
        <ul>
          {order.products.map((p, idx) => (
            <li key={`${p.productId}-${idx}`}>
              {p.name} - {p.quantity} x ${p.unitPrice}
            </li>
          ))}
        </ul>
        <p><strong>Estado:</strong> <StatusRenderer value={status} /></p>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>Cerrar</Button>
      </Modal.Footer>
    </Modal>
  );
}
