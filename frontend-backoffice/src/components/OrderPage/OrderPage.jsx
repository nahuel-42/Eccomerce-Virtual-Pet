import React from 'react';
import OrdersTable from './components/OrdersTable/OrdersTable';
import './OrderPage.css';

export default function OrdersPage() {
  return (
    <div className="table-body">
      <div className="w-fit">
        <h1 className="title mb-4 text-left">Gestión de Pedidos</h1>
        <OrdersTable />
      </div>
    </div>
  );
}
