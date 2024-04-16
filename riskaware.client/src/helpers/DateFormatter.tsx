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

const DateInputFormatter: Intl.DateTimeFormat = new Intl.DateTimeFormat("cs-CZ", {
  year: "numeric",
  month: "2-digit",
  day: "2-digit",
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

export function formatDateForInput(date: Date) {
  const formattedDate = date instanceof Date ? DateInputFormatter.format(date)
    : DateInputFormatter.format(new Date(date));

  // Split the formatted date by space and reorder it to "yyyy-MM-dd"
  const parts = formattedDate.split('.').map(part => part.trim());
  const [day, month, year] = parts;

  // Ensure that the parts are in "yyyy-MM-dd" format
  return `${year}-${month}-${day}`;
}
