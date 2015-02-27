  (function( $ ) {
        $.widget("custom.combobox", {
            options: {
                dataUri: '',
                onchange: function (){},
                createRenderItem: function (ul, item) {
                    return $("<li>").attr("data-value", item.value).append(item.label).appendTo(ul);
                }
            },
        _create: function() {
                this.wrapper = $( "<span>" )
                .addClass( "custom-combobox" )
                .insertAfter( this.element );
                this.element.hide();
                this._createAutocomplete();
                this._createShowAllButton();
            },
            _createAutocomplete: function() {
                var selected = this.element.children(":selected");
                var value = selected.val() ? selected.val() : "";
                var text = selected.val() ? selected.text() : "";
                var id = $(this.element).attr('id');
                var name = $(this.element).attr('name');
                $(this.element).attr('id', '');
                $(this.element).attr('name', '');
                var that = this;
                this.hidden = $('<input>').attr('id', id).attr('name', name).attr('type', 'hidden').appendTo(this.wrapper).val(value);
                this.input = $( "<input>" )
                .appendTo( this.wrapper )
                .val( text )
                .attr("title", "")
                .attr('id', id + "Text")
                .addClass( "custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left" )
                .autocomplete({
                    delay: 1000,
                    minLength: 0,
                    source: this.options.dataUri(),
                    create: function() {
                        $(this).data('ui-autocomplete')._renderItem = that.options.createRenderItem;
                    }
                }).tooltip({
                    tooltipClass: "ui-state-highlight"
                });

                this._on( this.input, {
                    autocompleteselect: function (event, ui) {
                        event.preventDefault();
                        $(this.hidden).val(ui.item.value);
                        $(this.input).val(ui.item.label);
                        this.options.onchange();
                    },
                    autocompletesearch: function (event, ui) {
                        this.input.autocomplete( "option", "source", this.options.dataUri());
                    }
                });
            },
            _createShowAllButton: function() {
                var input = this.input,
                wasOpen = false;
                $( "<a>" )
                .attr( "tabIndex", -1 )
                .attr( "title", "Show All Items" )
                .appendTo( this.wrapper )
                .button({
                    text: false,
                    icons: {
                        primary: "ui-icon-triangle-1-s"
                    }
                })
                .removeClass( "ui-corner-all" )
                .addClass( "custom-combobox-toggle ui-corner-right" )
                .mousedown(function() {
                    wasOpen = input.autocomplete( "widget" ).is( ":visible" );
                })
                .click(function() {
                    input.focus();
                    // Close if already visible
                    if ( wasOpen ) {
                        return;
                    }
                    // Pass empty string as value to search for, displaying all results
                    input.autocomplete( "search", "" );
                });
            },
            _destroy: function() {
                this.wrapper.remove();
                this.element.show();
            }
        });
    })( jQuery );