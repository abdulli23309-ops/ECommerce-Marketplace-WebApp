export const getImageUrl = (relativeUrl) => {
  if (!relativeUrl) return null;
  if (relativeUrl.startsWith("http")) return relativeUrl;
  // Remove trailing /api from the API base URL to get the root URL
  const base = import.meta.env.VITE_API_BASE_URL?.replace(/\/api$/, "") || "";
  return `${base}${relativeUrl}`;
};