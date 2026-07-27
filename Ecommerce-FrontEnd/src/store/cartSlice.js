import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import {
  fetchCart,
  addToCart,
  updateCartItemQuantity,
  removeCartItem,
  clearCart,
} from "../services/cartService";

// Async thunks
export const loadCart = createAsyncThunk("cart/loadCart", async () => {
  const cart = await fetchCart();
  return cart; // { cartId, items: [...] }
});

export const addItemToCart = createAsyncThunk(
  "cart/addItem",
  async ({ productId, quantity }) => {
    const cart = await addToCart(productId, quantity);
    return cart;
  }
);

export const updateQuantity = createAsyncThunk(
  "cart/updateQuantity",
  async ({ cartItemId, quantity }) => {
    const cart = await updateCartItemQuantity(cartItemId, quantity);
    return cart;
  }
);

export const removeFromCart = createAsyncThunk(
  "cart/removeItem",
  async (cartItemId) => {
    await removeCartItem(cartItemId);
    // Return the updated cart after deletion
    const cart = await fetchCart();
    return cart;
  }
);

export const emptyCart = createAsyncThunk("cart/clear", async () => {
  await clearCart();
  return { items: [] };
});

const cartSlice = createSlice({
  name: "cart",
  initialState: {
    items: [],
    totalCount: 0,
    status: "idle", // 'idle' | 'loading' | 'succeeded' | 'failed'
    error: null,
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(loadCart.pending, (state) => {
        state.status = "loading";
      })
      .addCase(loadCart.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.items = action.payload?.items || [];
        state.totalCount = state.items.reduce(
          (sum, item) => sum + item.quantity,
          0
        );
      })
      .addCase(loadCart.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.error.message;
      })
      .addCase(addItemToCart.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.items = action.payload?.items || [];
        state.totalCount = state.items.reduce(
          (sum, item) => sum + item.quantity,
          0
        );
      })
      .addCase(updateQuantity.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.items = action.payload?.items || [];
        state.totalCount = state.items.reduce(
          (sum, item) => sum + item.quantity,
          0
        );
      })
      .addCase(removeFromCart.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.items = action.payload?.items || [];
        state.totalCount = state.items.reduce(
          (sum, item) => sum + item.quantity,
          0
        );
      })
      .addCase(emptyCart.fulfilled, (state) => {
        state.status = "succeeded";
        state.items = [];
        state.totalCount = 0;
      });
  },
});

export default cartSlice.reducer;