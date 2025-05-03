import React from 'react';
import { ORDER_STATUS_OPTIONS, OrderStatus, ORDER_STATUS_COLORS } from '../../constants/order-status';

const StatusRenderer = ({ value }) => {
  const label = ORDER_STATUS_OPTIONS.find(opt => opt.value === value)?.label || value;
  const bgColor = ORDER_STATUS_COLORS[value] || 'bg-secondary';

  return (
    <span className={`badge ${bgColor} text-white px-3 py-2 rounded-pill text-uppercase`}>
      {label}
    </span>
  );
};

export default StatusRenderer;

