import React, { useEffect, useState, useMemo } from 'react';
import { getOrders } from '../../../../services/order.service';
import OrderDetailModal from '../OrderDetailModal/OrderDetailModal';
import { AgGridReact } from 'ag-grid-react';
import { AllCommunityModule, ModuleRegistry, themeAlpine, themeBalham, themeMaterial, themeQuartz } from 'ag-grid-community'; 
import StatusRenderer from '../StatusRenderer/StatusRenderer'

ModuleRegistry.registerModules([AllCommunityModule]);

export default function OrdersTable() {
  const [rowData, setRowData] = useState([]);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [showModal, setShowModal] = useState(false);

  const columnDefs = useMemo(() => [
    { field: 'id', headerName: 'ID', width: 90, filter: true },
    {
      field: 'createdDate',
      headerName: 'Fecha Pedido',
      valueFormatter: (params) => new Date(params.value).toLocaleString(),
      width: 200,
      filter: true,
    },
    {
      field: 'status',
      headerName: 'Estado',
      width: 220,
      cellRenderer: StatusRenderer,
      filter: true,
      sortable: true,
    },    
    { field: 'phone', headerName: 'Teléfono', width: 200, filter: true },
    { field: 'address', headerName: 'Dirección', width: 250, filter: true },
    {
      headerName: 'Cliente',
      valueGetter: (params) => params.data.user?.name,
      width: 200,
      filter: true,
    },
    {
      headerName: 'Ver Detalles',
      cellRenderer: (params) => (
        <button
          className="btn btn-outline-primary btn-sm"
          onClick={() => handleDetailClick(params.data)}
        >
          Ver
        </button>
      ),
      width: 150,
      sortable: false,
      filter: false,
    }      
  ], []);

  const handleDetailClick = (order) => {
    setSelectedOrder(order);
    setShowModal(true);
  };

  useEffect(() => {
    getOrders().then((res) => {
      setRowData(res.data);
    });
  }, []);

  return (
    <div style={{ height: 600, width: 1290}}>
      <AgGridReact theme={themeAlpine} rowData={rowData} columnDefs={columnDefs} />
      {selectedOrder && (
        <OrderDetailModal
          orderId={selectedOrder.id}
          show={showModal}
          onHide={() => setShowModal(false)}
        />
      )}
    </div>
  );
}
