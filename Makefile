all: build

build:
	$(MAKE) -C binding
	$(MAKE) -C forms

clean:
	$(MAKE) clean -C binding
	$(MAKE) clean -C forms
