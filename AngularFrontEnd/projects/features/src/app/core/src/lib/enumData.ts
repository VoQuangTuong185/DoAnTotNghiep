export const enumData = {
  /** Trạng thái đon hàng */
  statusOrder: {
    wait: { value: 0, code: 'wait', name: 'Chờ xác nhận' },
    processing: { value: 1, code: 'processing', name: 'Đang giao hàng' },
    success: { value: 2, code: 'success', name: 'Thành công' },
    cancel: { value: 3, code: 'cancel', name: 'Đã hủy' },
  },
};
