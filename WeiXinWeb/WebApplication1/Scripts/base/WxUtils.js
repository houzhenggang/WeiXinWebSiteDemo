var wxUtils = {};

wxUtils.Ajax= function (ajaxUrl, ajaxData,requestType,dataType, success) {
        $.ajax({
            async: true,
            url: ajaxUrl,
            type: requestType,
            data: ajaxData,
            cache: false,
            dataType: dataType,
            success: function (data, textStatus, jqXHr) {
                if ($.isFunction(success)) success(data, textStatus, jqXHr);
            }
          
        });
    };