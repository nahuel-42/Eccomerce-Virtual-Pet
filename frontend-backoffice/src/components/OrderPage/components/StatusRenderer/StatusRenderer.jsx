import React from 'react';
import {
  ORDER_STATUS_OPTIONS,
  ORDER_STATUS_COLORS,
  OrderStatus
} from '../../constants/order-status';

const StatusRenderer = ({ value }) => {
  const option = ORDER_STATUS_OPTIONS.find(opt => opt.value === value);
  const label = option?.label || `${value}`;

  const statusKey = Object.keys(OrderStatus).find(
    key => OrderStatus[key] === value
  );

  const bgColor = ORDER_STATUS_COLORS[statusKey] || 'bg-secondary';

  return (
    <span className={`badge ${bgColor} text-black px-3 py-2 rounded-pill text-uppercase`}>
      {label}
    </span>
  );
};

export default StatusRenderer;
