STACK_GEM_VERSION = '1.7.0' unless defined? STACK_GEM_VERSION

require File.join(File.dirname(__FILE__), 'boot')

Stack::Initializer.run do |config|
  # Defines the development team that ownes the product.
  # This group must be defined as a group in Subversion.
  config.owning_team = 'Transportation'

  # Custom ignore patterns. 
  # 
  # These patterns are used in both the 
  # svn:ignore and svn:clean tasks.
  # 
  # config.root_ignore_patterns += %w(*.resharper *.resharper.* _ReSharper*)
  # 
  # Ignore patterns that apply to the out directory.
  # 
  # config.out_ignore_patterns += %w()
  # 
  # Ignore patterns that apply to all Visual Studio.Net
  # project directories.
  # 
  # config.project_ignore_patterns += %w()
end