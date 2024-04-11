// For better performance and zero padding datetime
const DateFormatter: Intl.DateTimeFormat = new Intl.DateTimeFormat("cs-CZ", {
  year: "numeric",
  month: "2-digit",
  day: "2-digit",
  hour12: false,
  timeZone: 'Europe/Prague'
});

const DateTimeFormatter: Intl.DateTimeFormat = new Intl.DateTimeFormat("cs-CZ", {
  year: "numeric",
  month: "2-digit",
  day: "2-digit",
  hour: "2-digit",
  minute: "2-digit",
  hour12: false,
  timeZone: 'Europe/Prague'
});

// Format Date in czech format
export function formatDate(date: Date) {
  return date instanceof Date ? DateFormatter.format(date)
    : DateFormatter.format(new Date(date));
}

// Format DateTime in czech format
export function formatDateTime(date: Date) {
  return date instanceof Date ? DateTimeFormatter.format(date)
    : DateTimeFormatter.format(new Date(date));
}
