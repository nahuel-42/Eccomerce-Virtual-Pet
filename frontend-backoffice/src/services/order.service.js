import axios from 'axios';
import API_BASE_URL from './config';

const API_URL = `${API_BASE_URL}/orders`;

const authHeaders = () => ({
  headers: {
    Authorization: `Bearer ${localStorage.getItem('token')}`,
  },
});

export const getOrders = () => axios.get(API_URL, authHeaders());

export const getOrderById = (id) => axios.get(`${API_URL}/${id}`, authHeaders());

export const updateOrder = (id, payload) => axios.put(`${API_URL}/${id}`, payload, authHeaders());
