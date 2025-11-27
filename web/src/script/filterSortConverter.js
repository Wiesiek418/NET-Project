function convertFilters(filters){
    const opsMap = [
    { regex: /^>=/, op: 'gt' },
    { regex: /^<=/, op: 'lte' },
    { regex: /^>/, op: 'gt' },
    { regex: /^</, op: 'lt' },
    { regex: /^=/, op: 'eq' }
  ];

   const parts = filters.map(f => {
    let val = f.value.toString();
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

    return `${encodeURIComponent(f.field)}:${op}:${encodeURIComponent(val)}`;
  });

  return parts.join(',');
}

function convertSort(sort) {
  if (!sort || !sort.length) return '';
  return sort.map(s => `${encodeURIComponent(s.field)}:${s.direction}`).join(',');
}