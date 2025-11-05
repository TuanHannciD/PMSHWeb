function formatDateForInput(dateStr) {
    const parts = dateStr.trim().split(' ')[0].split('/'); // Lấy phần ngày: "22/10/2025"
    if (parts.length === 3) {
        const [dd, mm, yyyy] = parts;
        return `${yyyy}-${mm.padStart(2, '0')}-${dd.padStart(2, '0')}`;
    }
    return '';
}