
export interface FeedbackDetailShow {
    userId: number;
    productId: number;
    loginName?: string;
    userName?: string;
    comments?: string;
    votes: number;
    orderId: number;
    createdDate?: Date;
    adminReply?: string;
    replyDate?: Date;
}