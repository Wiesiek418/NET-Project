export function convertFilters(filters){
    const opsMap = [
    { regex: /^>=/, op: 'gt' },
    { regex: /^<=/, op: 'lte' },
    { regex: /^>/, op: 'gt' },
    { regex: /^</, op: 'lt' },
    { regex: /^=/, op: 'eq' }
  ];

   const parts = Object.entries(filters)
    .filter(([key, value]) => value !== null && value !== undefined && value !== '')
    .map(([key, val]) => {
      val = val.toString();
      let op = 'eq'; // domyślny operator

      if (val.toLowerCase() === 'auto') {
        op = 'eq';
      } else {
        for (const { regex, op: mappedOp } of opsMap) {
          if (regex.test(val)) {
            op = mappedOp;
            val = val.replace(regex, '');
            break;
          }
        }
      }

      return `${encodeURIComponent(key)}:${op}:${encodeURIComponent(val)}`;
    });

  return parts.join(',');
}

export function convertSort(sort) {
  if (!sort || !sort.field) return '';
  return `${encodeURIComponent(sort.field)}:${sort.direction}`;
}

export function parseDate(value){

  
}