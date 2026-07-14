function initializeDataTable(selector, options = {}) {

    const table = document.querySelector(selector);

    if (!table) {
        return;
    }

    const defaultOptions = {

        language: {
            url: "/plugins/datatables/lang/tr.json"
        },

        responsive: true,

        pageLength: 10,

        lengthMenu: [
            [10, 25, 50, 100],
            [10, 25, 50, 100]
        ],

        ordering: true,

        searching: true,

        info: true,

        autoWidth: false

    };

    new DataTable(table, {

        ...defaultOptions,

        ...options

    });

}