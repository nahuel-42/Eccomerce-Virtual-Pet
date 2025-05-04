import React, { useEffect, useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import { getOrderById, updateOrder } from '../../../../services/order.service';
import { ORDER_STATUS_OPTIONS } from '../../constants/order-status';

export default function OrderDetailModal({ orderId, show, onHide, onOrderUpdated }) {
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

  const handleSave = () => {
    updateOrder(orderId, { orderStatusId: status }).then(() => {
      onHide();
      onOrderUpdated();
    });
  };

  if (!order) return null;

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

        <Form.Group>
          <Form.Label>Estado</Form.Label>
          <Form.Select value={status} onChange={(e) => setStatus(Number(e.target.value))}>
            {ORDER_STATUS_OPTIONS.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </Form.Select>
        </Form.Group>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>Cancelar</Button>
        <Button variant="primary" onClick={handleSave}>Guardar Cambios</Button>
      </Modal.Footer>
    </Modal>
  );
}