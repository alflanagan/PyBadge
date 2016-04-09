from setuptools import setup
setup(name='PyBadge',
      version='0.1',
      description='Python USB interface to RVAsec Badge',
      author='Morgan Stuart',
      install_requires = ['pyserial'],
      py_modules = ['ForthBBProtocol'])
        #packages=['dnn_project'])
